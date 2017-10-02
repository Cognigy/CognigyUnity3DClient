using Cognigy.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quobject.EngineIoClientDotNet.Client.Transports;
using Quobject.SocketIoClientDotNet.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace Cognigy
{
    public class AIClient : CognigyClient
    {
        public event EventHandler<OutputEventArgs> OnOutput;
        public event EventHandler<StepEventArgs> OnStep;

        private AIOptions aiOptions;
        private bool firstLoad;

        private Func<object, Output> BuildOutputObject = data => JsonConvert.DeserializeObject<Output>(Convert.ToString(data));
        private Func<object, Step> BuildStepObject = data => JsonConvert.DeserializeObject<Step>(Convert.ToString(data));

        public AIClient(AIOptions aiOptions)
        {
            this.aiOptions = aiOptions;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            ServicePointManager.ServerCertificateValidationCallback = ServerCertificateValidator.ValidateCertificate;

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Newtonsoft.Json.Formatting.Indented,
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            };
        }

        public override void Connect(CancellationToken cxToken, int millisecondsTimeout)
        {
            this.cxToken = cxToken;
            this.millisecondsTimeout = millisecondsTimeout;

            this.Cancelable = true;
            if (!cxToken.IsCancellationRequested)
            {
                Initialized = InitCognigyClient(
                    EstablishSocketConnection(
                        GetToken(
                            this.aiOptions.AIServerUrl,
                            this.aiOptions.User,
                            this.aiOptions.APIKey,
                            this.aiOptions.Channel,
                            this.aiOptions.Token
                            )));
                this.Cancelable = false;
            }
            else
            {
                throw new CognigyOperationCanceledException();
            }
        }

        public void SendMessage<T>(string text, T data)
        {
            if (this.IsConnected())
            {
                RawMessage<T> rawMessage = new RawMessage<T>(text, data);
                this.mySocket.Emit("input", JObject.FromObject(rawMessage));
            }
            else
            {
                throw new CognigyConnectionException(ConnectionErrorType.NoConnection, "Socket is not connected");
            }
        }

        public void ResetFlow(string newFlowId, string language, float version)
        {
            if (this.IsConnected())
            {
                Dictionary<string, object> resetFlowParam = new Dictionary<string, object>()
            {
                {"id", newFlowId},
                {"language", language},
                {"version", version}
            };
                this.mySocket.Emit("resetFlow", JObject.FromObject(resetFlowParam));
            }
            else
                throw new CognigyConnectionException(ConnectionErrorType.NoConnection, "Socket is not connected");
        }

        public void RestState()
        {
            if (this.IsConnected())
                this.mySocket.Emit("resetState");
            else
                throw new CognigyConnectionException(ConnectionErrorType.NoConnection, "Socket is not connected");
        }

        public void ResetContext()
        {
            if (this.IsConnected())
                this.mySocket.Emit("resetContext");
            else
                throw new CognigyConnectionException(ConnectionErrorType.NoConnection, "Socket is not connected");
        }

        private string GetToken(string baseUrl, string user, string apikey, string channel, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                return token;
            }
            else
            {
                return Fetch(baseUrl, user, apikey, channel);
            }
        }

        private string Fetch(string baseUrl, string user, string apikey, string channel)
        {
            if (!cxToken.IsCancellationRequested)
            {
                string jsonString = JsonConvert.SerializeObject(new RequestBodyContent(user, apikey, channel));
                byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonString);

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(baseUrl + "/loginDevice");
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = bodyRaw.Length;
                request.Accept = "application/json";

                Stream reqStream = request.GetRequestStream();
                reqStream.Write(bodyRaw, 0, bodyRaw.Length);
                reqStream.Close();

                if (!cxToken.IsCancellationRequested)
                {
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            Stream respStream = response.GetResponseStream();
                            StreamReader respStreamReader = new StreamReader(respStream, Encoding.Default);

                            ResponseBodyContent resBodyJson = JsonConvert.DeserializeObject<ResponseBodyContent>(respStreamReader.ReadToEnd());
                            string token = resBodyJson.token;

                            respStreamReader.Close();
                            respStream.Close();
                            response.Close();

                            if (!string.IsNullOrEmpty(token))
                            {
                                request.Abort();
                                return token;
                            }
                            else
                            {
                                request.Abort();
                                throw new CognigyRequestException("No token received");
                            }
                        }
                        else
                        {
                            request.Abort();
                            response.Close();
                            throw new CognigyRequestException("Status Code: " + response.StatusCode.ToString());
                        }
                    }
                }
                else
                {
                    throw new CognigyOperationCanceledException();
                }
            }
            else
            {
                throw new CognigyOperationCanceledException();
            }
        }

        private Socket EstablishSocketConnection(string token)
        {
            if (!cxToken.IsCancellationRequested)
            {
                var options = new IO.Options()
                {
                    Reconnection = true,
                    AutoConnect = true,
                    QueryString = "token=" + token,
                    Upgrade = true,
                    ForceNew = true,
                    Multiplex = false,
                    Transports = new List<string> { WebSocket.NAME, Polling.NAME }
                };

                this.mySocket = IO.Socket(new Uri(this.aiOptions.AIServerUrl), options);

                this.mySocket.On("connect", () =>
                {
                    this.isConnected = true;
                    waitHandle.Set();
                });

                this.mySocket.On("connect_error", (data) => { throw new CognigyConnectionException(ConnectionErrorType.ConnectionError, Convert.ToString(data)); });
                this.mySocket.On("connect_timeout", (data) => { throw new CognigyConnectionException(ConnectionErrorType.ConnectionTimeout, Convert.ToString(data)); });

                this.mySocket.On("error", (data) => { throw new CognigyAIException(AIErrorType.Error, Convert.ToString(data)); });
                this.mySocket.On("exception", (data) => { throw new CognigyAIException(AIErrorType.Exception, Convert.ToString(data)); });

                this.mySocket.On("disconnect", (data) => this.isConnected = false);

                this.mySocket.On("output", (data) =>
                {
                    if (OnOutput != null)
                        OnOutput(this, new OutputEventArgs(BuildOutputObject(data)));
                });

                this.mySocket.On("logStep", (data) =>
                {
                    if (OnStep != null)
                        OnStep(this, new StepEventArgs(BuildStepObject(data)));
                });

                if (CustomWaitHandle.CancelableWaitOne(waitHandle, millisecondsTimeout, cxToken))
                {
                    return this.mySocket;
                }
                else //Timeout
                {
                    Disconnect();
                    throw new CognigyConnectionException(ConnectionErrorType.ConnectionTimeout, "No answer from server received");
                }
            }
            else
            {
                throw new CognigyOperationCanceledException();
            }
        }

        private bool InitCognigyClient(Socket socket)
        {
            if (!cxToken.IsCancellationRequested)
            {
                int? version = null;

                if (this.aiOptions.Version != 0)
                    version = this.aiOptions.Version;

                InitializationParameters initParam = new InitializationParameters(
                    this.aiOptions.Flow,
                    this.aiOptions.Language.ToString().Replace("_", "-"),
                    version,
                    this.aiOptions.PassthroughIP,
                    this.aiOptions.ResetState,
                    this.aiOptions.ResetContext
                    );

                socket.Emit("init", JObject.FromObject(initParam));

                socket.On("initResponse", () => waitHandle.Set());
                socket.On("exception", () => { throw new CognigyAIException(AIErrorType.Exception, "Error in brain initialization"); });

                if (CustomWaitHandle.CancelableWaitOne(waitHandle, millisecondsTimeout, cxToken))
                {
                    return true;
                }
                else
                {
                    Disconnect();
                    throw new CognigyConnectionException(ConnectionErrorType.ConnectionTimeout, "No answer from server received");
                }
            }
            else
            {
                throw new CognigyOperationCanceledException();
            }
        }
    }
}

