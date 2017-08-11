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
            : base(BuildMessage(connectionErrorType, message))
        {
        }

        public CognigyConnectionException(string message, Exception inner)
            : base(message, inner)
        {
        }

        private static string BuildMessage(ConnectionErrorType connectionErrorType, string message)
        {
            return string.Format("-- {0} -- {1}", connectionErrorType.ToString(), message);
        }
    }
}
