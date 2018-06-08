using Cognigy.Utility;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Cognigy
{
    [AddComponentMenu("Cognigy/COGNIGY.AI")]
    public class CognigyAI : MonoBehaviour
    {
        public SocketEndpointOptions socketEndpointOptions;

        public bool IsConnected
        {
            get
            {
                if (aiClient != null)
                    return aiClient.Initialized;
                else
                    return false;
            }
        }

        public event EventHandler<FlowOutputEventArgs> OnOutput;

        private AIClient aiClient;

        private bool hasSubscribed;

        private List<FlowOutputEventArgs> flowOutputsForMain = new List<FlowOutputEventArgs>();
        private List<FlowOutputEventArgs> flowOutputsCopiedForMain = new List<FlowOutputEventArgs>();

        private volatile bool noFlowOutputForMain = true;

        private CancellationTokenSource aiCancelTokenSource;

        public void ConnectAIClient()
        {
            if (socketEndpointOptions != null)
            {
                int millisecondsTimeout;

                if (socketEndpointOptions.MillisecondsTimeout <= 0)
                    millisecondsTimeout = Timeout.Infinite;
                else
                    millisecondsTimeout = socketEndpointOptions.MillisecondsTimeout;

                if (aiClient != null)
                    aiClient.Disconnect();

                aiClient = new AIClient(this.socketEndpointOptions);

                aiClient.OnOutput += StoreOutputEvent;

                hasSubscribed = true;

                aiCancelTokenSource = new CancellationTokenSource();
                ClientConnectionPacket aiClientPacket = new ClientConnectionPacket(this.aiClient, aiCancelTokenSource.Token, millisecondsTimeout);

                ThreadPool.QueueUserWorkItem(ConnectClient, aiClientPacket);
            }
            else
            {
                throw new CognigyNullOptionsException();
            }
        }

        public void DisconnectClient()
        {
            CancelClientConnection();

            if (aiClient != null)
            {
                if (hasSubscribed)
                {
                    aiClient.OnOutput -= StoreOutputEvent;
                    hasSubscribed = false;
                }

                aiClient.Disconnect();
            }
        }

        public void CancelClientConnection()
        {
            if (aiClient != null)
            {
                if (aiClient.Cancelable)
                    aiCancelTokenSource.Cancel();
            }
        }

        public void AISendMessage(string message)
        {
            if (aiClient != null && aiClient.IsConnected())
            {
                    ThreadPool.QueueUserWorkItem((cb) =>
                    {
                        aiClient.SendMessage<object>(message, null);
                    });
            }
            else
            {
                Debug.LogError("-- [COGNGIY.AI] Client not connected --");
            }
        }

        public void AISendMessage<T>(T data)
        {
            if (aiClient != null && aiClient.IsConnected())
            {
                    ThreadPool.QueueUserWorkItem((cb) =>
                    {
                        aiClient.SendMessage<object>(null, data);
                    });
            }
            else
            {
                Debug.LogError("-- [COGNGIY.AI] Client not connected --");
            }
        }

        public void AISendMessage<T>(string message, T data)
        {
            if (aiClient != null && aiClient.IsConnected())
            {
                    ThreadPool.QueueUserWorkItem((cb) =>
                    {
                        aiClient.SendMessage<object>(message, data);
                    });
            }
            else
            {
                Debug.LogError("-- [COGNGIY.AI] Client not connected --");
            }
        }

        private void ConnectClient(object stateInfo)
        {
            ClientConnectionPacket clientPacket = (ClientConnectionPacket)stateInfo;
            clientPacket.Client.Connect(clientPacket.Token, clientPacket.MillisecondsTimeout);
        }

        private void StoreOutputEvent(object sender, FlowOutputEventArgs args)
        {
            if (args == null)
                throw new ArgumentNullException("FlowOutputEventArgs");

            lock (flowOutputsForMain)
            {
                flowOutputsForMain.Add(args);
                noFlowOutputForMain = false;
            }
        }

        private void Update()
        {
            if (!noFlowOutputForMain)
            {
                flowOutputsCopiedForMain.Clear();

                lock (flowOutputsForMain)
                {
                    flowOutputsCopiedForMain.AddRange(flowOutputsForMain);
                    flowOutputsForMain.Clear();

                    noFlowOutputForMain = true;
                }

                for (int i = 0; i < flowOutputsCopiedForMain.Count; i++)
                {
                    if (OnOutput != null)
                        OnOutput(this, flowOutputsCopiedForMain[i]);
                }
            }
        }

        private void OnEnable()
        {
            if (aiClient != null && IsConnected && !hasSubscribed)
            {
                aiClient.OnOutput += StoreOutputEvent;

                hasSubscribed = true;
            }
        }

        private void OnDisable()
        {
            if (aiClient != null && hasSubscribed)
            {
                aiClient.OnOutput -= StoreOutputEvent;

                hasSubscribed = false;
            }
        }

        private void OnApplicationQuit()
        {
            DisconnectClient();
        }
    }
}
