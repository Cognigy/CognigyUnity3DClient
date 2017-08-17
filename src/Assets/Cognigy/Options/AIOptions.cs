using System;
using UnityEngine;

namespace Cognigy
{
    public enum AILanguage
    {
        en_US,
        de_DE
    }

    [Serializable]
    [CreateAssetMenu(fileName = "Cognigy_AI_Options", menuName = "Cognigy/Cognigy AI Options", order = 1)]
    public class AIOptions : ScriptableObject
    {
        [SerializeField]
        public string APIKey = string.Empty;
        [SerializeField]
        public string AIServerUrl = string.Empty;
        [SerializeField]
        public int MillisecondsTimeout;
        [Space]
        [SerializeField]
        public string User = string.Empty;
        [SerializeField]
        public string Flow = string.Empty;
        [SerializeField]
        public AILanguage Language = AILanguage.en_US;
        [Header("Optional")]
        [SerializeField]
        public string Token = string.Empty;
        [SerializeField]
        public int Version = 0;
        [SerializeField]
        public string Channel = string.Empty;
        [SerializeField]
        public bool Reconnection = true;
        [SerializeField]
        public bool ListenToStep = false;
        [SerializeField]
        public int Interval = 10000;
        [SerializeField]
        public bool ResetState = false;
        [SerializeField]
        public bool ResetContext = false;
        [SerializeField]
        public string PassthroughIP = string.Empty;
    }
}
