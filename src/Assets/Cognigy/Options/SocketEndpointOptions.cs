using System;
using UnityEngine;

namespace Cognigy
{
    [Serializable]
    [CreateAssetMenu(fileName = "CognigySocketEndpointOptions", menuName = "Cognigy/Cognigy Socket Options", order = 1)]
    public class SocketEndpointOptions : ScriptableObject
    {
        [SerializeField]
        public string EndpointURL = string.Empty;
        [SerializeField]
        public string URLToken = string.Empty;
        [SerializeField]
        public string UserID = string.Empty;
        [SerializeField]
        public string SessionID = string.Empty;
        [SerializeField]
        public int MillisecondsTimeout = 0;
        [SerializeField]
        public bool Reconnection = true;
        [SerializeField]
        public bool ListenToStep = false;
        [SerializeField]
        public bool ResetState = false;
        [SerializeField]
        public bool ResetContext = false;
        [SerializeField]
        public string PassthroughIP = string.Empty;
    }
}
