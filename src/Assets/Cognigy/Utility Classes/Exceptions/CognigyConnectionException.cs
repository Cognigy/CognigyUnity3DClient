using System;

namespace Cognigy.Utility
{
    public enum ConnectionErrorType
    {
        NoConnection,
        ConnectionError,
        ConnectionTimeout,
    }

    [Serializable]
    public class CognigyConnectionException : CognigyException
    {
        public ConnectionErrorType ConnectionErrorType { get; private set; }
        public CognigyConnectionException() { }

        public CognigyConnectionException(ConnectionErrorType connectionErrorType, string message)
            : base(message)
        {
        }

        public CognigyConnectionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
