using System;

namespace Cognigy.Utility
{
    [Serializable]
    public class CognigyException : Exception
    {
        public CognigyException() { }

        public CognigyException(string message)
            : base(message)
        {
        }

        public CognigyException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
