
using System;

namespace Cognigy.Utility
{
    public enum AIErrorType
    {
        Exception,
        Error
    }
    public class CognigyAIException : CognigyException
    {
        public CognigyAIException() { }

        public CognigyAIException(AIErrorType aiErrorType, string message)
            : base(BuildMessage(aiErrorType, message))
        {
        }

        public CognigyAIException(string message, Exception inner)
            : base(message, inner)
        {
        }

        private static string BuildMessage(AIErrorType aiErrorType, string message)
        {
            return string.Format("-- {0} -- {1}", aiErrorType.ToString(), message);
        }
    }
}
