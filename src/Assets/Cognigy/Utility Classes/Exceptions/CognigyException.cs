using System;
using UnityEngine;

namespace Cognigy.Utility
{
    [Serializable]
    public class CognigyException : Exception
    {
        public CognigyException() { }

        public CognigyException(string message)
            : base(message)
        {
            Debug.LogError(string.Format("-- {0} {1} -- \n\n {2}","[COGNIGY.AI]", message, this.StackTrace));
        }

        public CognigyException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
