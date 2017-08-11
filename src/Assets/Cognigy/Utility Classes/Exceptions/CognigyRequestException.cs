using System;

namespace Cognigy.Utility
{
    [Serializable]
    public class CognigyRequestException : CognigyException
    {
        public CognigyRequestException() { }

        public CognigyRequestException(string message)
                : base(message)
        {
        }

        public CognigyRequestException(string message, Exception inner)
                : base(message, inner)
        {
        }
    }
}
