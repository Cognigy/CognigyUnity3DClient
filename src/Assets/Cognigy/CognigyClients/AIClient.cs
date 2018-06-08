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
using UnityEngine;

namespace Cognigy
{
    public class AIClient : CognigyClient
    {
        private const string EVENT_PROCESS_INPUT = "processInput";
        private const string EVENT_EXCEPTION = "exception";
        private const string EVENT_OUTPUT = "output";

        public event EventHandler<FlowOutputEventArgs> OnOutput;

        private Func<object, FlowOutput> BuildFlowOutput = data => JsonConvert.DeserializeObject<FlowOutput>(Convert.ToString(data));

        private SocketEndpointOptions socketEndpointOptions;

        public AIClient(SocketEndpointOptions socketEndpointOptions)
        {
            this.socketEndpointOptions = socketEndpointOptions;

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
                Initialized = EstablishSocketConnection();
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
                CognigyInput<T> cognigyInput = new CognigyInput<T>
                (
                    this.socketEndpointOptions.URLToken, 
                    this.socketEndpointOptions.UserID, 
                    this.socketEndpointOptions.SessionID,
                    text, 
                    data
                );

                this.mySocket.Emit(EVENT_PROCESS_INPUT, JObject.FromObject(cognigyInput));
            }
            else
            {
                throw new CognigyConnectionException(ConnectionErrorType.NoConnection, "Socket is not connected");
            }
        }

        private bool EstablishSocketConnection()
        {
            if (!cxToken.IsCancellationRequested)
            {
                var options = new IO.Options()
                {
                    Reconnection = true,
                    AutoConnect = true,
                    QueryString = "UrlToken=" + this.socketEndpointOptions.URLToken,
                    Upgrade = true,
                    ForceNew = true,
                    Multiplex = false,
                    Transports = new List<string> { WebSocket.NAME, Polling.NAME }
                };

                this.mySocket = IO.Socket(new Uri(this.socketEndpointOptions.EndpointURL), options);

                this.mySocket.On(Socket.EVENT_CONNECT, () =>
                {
                    this.isConnected = true;
                    waitHandle.Set();
                });

                this.mySocket.On(Socket.EVENT_CONNECT_ERROR, (data) => { throw new CognigyConnectionException(ConnectionErrorType.ConnectionError, data.ToString()); });
                this.mySocket.On(Socket.EVENT_CONNECT_TIMEOUT, (data) => { Debug.LogError(data); throw new CognigyConnectionException(ConnectionErrorType.ConnectionTimeout, Convert.ToString(data)); });

                this.mySocket.On(Socket.EVENT_ERROR, (data) => { Debug.LogError(data); throw new CognigyAIException(AIErrorType.Error, Convert.ToString(data)); });
                this.mySocket.On(EVENT_EXCEPTION, (data) => { Debug.LogError(data); throw new CognigyAIException(AIErrorType.Exception, Convert.ToString(data)); });

                this.mySocket.On(Socket.EVENT_DISCONNECT, (data) => { Debug.Log("-- [COGNIGY.AI] Socket Client disconnected --"); this.isConnected = false; });

                this.mySocket.On(EVENT_OUTPUT, (data) =>
                {
                    if (OnOutput != null)
                    {
                        JObject response = JObject.FromObject(data);
                        if(response["type"].ToString() == "output")
                            OnOutput(this, new FlowOutputEventArgs(BuildFlowOutput(response["data"])));
                    }
                });

                if (CustomWaitHandle.CancelableWaitOne(waitHandle, millisecondsTimeout, cxToken))
                {
                    return true;
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
    }
}

