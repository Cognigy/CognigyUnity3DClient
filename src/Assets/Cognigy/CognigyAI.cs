using Cognigy.Utility;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Cognigy
{
    [AddComponentMenu("Cognigy/Cognigy AI")]
    public class CognigyAI : MonoBehaviour
    {
        public AIOptions aiOptions;

        public bool HasAI
        {
            get
            {
                if (aiClient != null)
                    return aiClient.Initialized;
                else
                    return false;
            }
        }

        public event EventHandler<OutputEventArgs> OnOutput;
        public event EventHandler<StepEventArgs> OnStep;

        private AIClient aiClient;

        private bool hasSubscribed;
        private bool listensToStepEvents;

        private static List<StepEventArgs> stepEventForMain = new List<StepEventArgs>();
        private List<StepEventArgs> stepEventCopiedForMain = new List<StepEventArgs>();

        private static List<OutputEventArgs> outputEventForMain = new List<OutputEventArgs>();
        private List<OutputEventArgs> outputEventCopiedForMain = new List<OutputEventArgs>();

        private volatile static bool noOutputEventForMain = true;
        private volatile static bool noStepEventForMain = true;

        private CancellationTokenSource aiCancelTokenSource;

        public void ConnectAIClient()
        {
            if (aiOptions != null)
            {
                int millisecondsTimeout;

                if (aiOptions.MillisecondsTimeout <= 0)
                    millisecondsTimeout = Timeout.Infinite;
                else
                    millisecondsTimeout = aiOptions.MillisecondsTimeout;

                if (aiClient != null)
                    aiClient.Disconnect();

                aiClient = new AIClient(this.aiOptions);

                aiClient.OnOutput += StoreOutputEvent;

                if (aiOptions.ListenToStep)
                {
                    listensToStepEvents = true;
                    aiClient.OnStep += StoreStepEvent;
                }

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
                    aiClient.OnStep -= StoreStepEvent;
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
                try
                {
                    ThreadPool.QueueUserWorkItem((cb) =>
                    {
                        aiClient.SendMessage<object>(message, null);
                    });
                }
                catch (Exception e)
                {

                    ErrorLogger.LogException(e);
                }
            }
            else
            {
                ErrorLogger.LogError("AI not connected");
            }
        }

        public void AISendMessage<T>(T data)
        {
            if (aiClient != null && aiClient.IsConnected())
            {
                try
                {
                    ThreadPool.QueueUserWorkItem((cb) =>
                    {
                        aiClient.SendMessage<object>(null, data);
                    });
                }
                catch (Exception e)
                {
                    ErrorLogger.LogException(e);
                }
            }
            else
            {
                ErrorLogger.LogError("AI not connected");
            }
        }

        public void AISendMessage<T>(string message, T data)
        {
            if (aiClient != null && aiClient.IsConnected())
            {
                try
                {
                    ThreadPool.QueueUserWorkItem((cb) =>
                    {
                        aiClient.SendMessage<object>(message, data);
                    });
                }
                catch (Exception e)
                {

                    ErrorLogger.LogException(e);
                }
            }
            else
            {
                ErrorLogger.LogError("AI not connected");
            }
        }

        private void ConnectClient(object stateInfo)
        {
            ClientConnectionPacket clientPacket = (ClientConnectionPacket)stateInfo;
            try
            {
                clientPacket.Client.Connect(clientPacket.Token, clientPacket.MillisecondsTimeout);
            }
            catch (Exception e)
            {
                ErrorLogger.LogException(e);
            }
        }

        private void StoreOutputEvent(object sender, OutputEventArgs args)
        {
            if (args == null)
                throw new ArgumentNullException("OutputEventArgs");

            lock (outputEventForMain)
            {
                outputEventForMain.Add(args);
                noOutputEventForMain = false;
            }
        }

        private void StoreStepEvent(object sender, StepEventArgs args)
        {
            if (args == null)
                throw new ArgumentNullException("StepEventArgs");

            lock (stepEventForMain)
            {
                stepEventForMain.Add(args);
                noStepEventForMain = false;
            }
        }

        private void Update()
        {
            if (!noOutputEventForMain)
            {
                outputEventCopiedForMain.Clear();

                lock (outputEventForMain)
                {
                    outputEventCopiedForMain.AddRange(outputEventForMain);
                    outputEventForMain.Clear();

                    noOutputEventForMain = true;
                }

                for (int i = 0; i < outputEventCopiedForMain.Count; i++)
                {
                    if (OnOutput != null)
                        OnOutput(this, outputEventCopiedForMain[i]);
                }
            }

            if (!noStepEventForMain)
            {
                stepEventCopiedForMain.Clear();

                lock (stepEventForMain)
                {
                    stepEventCopiedForMain.AddRange(stepEventForMain);
                    stepEventForMain.Clear();

                    noStepEventForMain = true;
                }

                for (int i = 0; i < stepEventCopiedForMain.Count; i++)
                {
                    if (OnStep != null)
                        OnStep(this, stepEventCopiedForMain[i]);
                }
            }
        }

        private void OnEnable()
        {
            if (aiClient != null && HasAI && !hasSubscribed)
            {
                aiClient.OnOutput += StoreOutputEvent;

                if (listensToStepEvents)
                    aiClient.OnStep += StoreStepEvent;

                hasSubscribed = true;
            }
        }

        private void OnDisable()
        {
            if (aiClient != null && hasSubscribed)
            {
                aiClient.OnOutput -= StoreOutputEvent;

                if (listensToStepEvents)
                    aiClient.OnStep -= StoreStepEvent;

                hasSubscribed = false;
            }
        }

        private void OnApplicationQuit()
        {
            DisconnectClient();
        }
    }
}
