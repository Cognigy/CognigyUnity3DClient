using Cognigy.Utility;
using Quobject.SocketIoClientDotNet.Collections.Concurrent;
using System;
using System.Collections;
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

        private ConcurrentQueue<OutputEventArgs> outputEventArgs = new ConcurrentQueue<OutputEventArgs>();
        private ConcurrentQueue<StepEventArgs> stepEventArgs = new ConcurrentQueue<StepEventArgs>();

        private CancellationTokenSource aiCancelTokenSource;

        private Coroutine aiPollingRoutine;

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
                aiClient.OnOutput += EnqueueOutput;
                aiClient.OnStep += EnqueueStep;
                hasSubscribed = true;

                aiCancelTokenSource = new CancellationTokenSource();
                ClientConnectionPacket aiClientPacket = new ClientConnectionPacket(this.aiClient, aiCancelTokenSource.Token, millisecondsTimeout);

                aiPollingRoutine = StartCoroutine(AIPollResult());

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
                if (aiPollingRoutine != null)
                    StopCoroutine(aiPollingRoutine);

                if (hasSubscribed)
                {
                    aiClient.OnOutput -= EnqueueOutput;
                    aiClient.OnStep -= EnqueueStep;
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
                    aiClient.SendMessage<object>(message.ToLower(), null);
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
                    aiClient.SendMessage(null, data);
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
                    aiClient.SendMessage(message.ToLower(), data);
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

        private void EnqueueOutput(object sender, OutputEventArgs args)
        {
            outputEventArgs.Enqueue(args);
        }

        private void EnqueueStep(object sender, StepEventArgs args)
        {
            stepEventArgs.Enqueue(args);
        }

        private IEnumerator AIPollResult()
        {
            while (aiClient != null)
            {
                OutputEventArgs outputArgs;
                while (outputEventArgs.TryDequeue(out outputArgs))
                {
                    if (OnOutput != null)
                        OnOutput(this, outputArgs);
                }

                StepEventArgs stepArgs;
                while (stepEventArgs.TryDequeue(out stepArgs))
                {
                    if (OnStep != null)
                        OnStep(this, stepArgs);
                }

                yield return null;
            }
        }

        private void OnEnable()
        {
            if (aiClient != null && HasAI && !hasSubscribed)
            {
                aiClient.OnOutput += EnqueueOutput;
                aiClient.OnStep += EnqueueStep;
                hasSubscribed = true;
            }
        }

        private void OnDisable()
        {
            if (aiClient != null && hasSubscribed)
            {
                aiClient.OnOutput -= EnqueueOutput;
                aiClient.OnStep -= EnqueueStep;
                hasSubscribed = false;
            }
        }

        private void OnApplicationQuit()
        {
            DisconnectClient();
        }
    }
}
