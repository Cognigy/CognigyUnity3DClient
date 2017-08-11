namespace Cognigy.Utility
{
    public class CognigyOperationCanceledException : CognigyException
    {
        private const string OperationCanceledMessage =
            "Operation has been canceled by the user";
        public CognigyOperationCanceledException() : base(OperationCanceledMessage) { }
    }
}
