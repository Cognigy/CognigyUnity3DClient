namespace Cognigy.Utility
{
    public class CognigyNullOptionsException : CognigyException
    {
        private const string NullOptionsMessage = "You need to assign valid Cognigy options.\n" +
                                            "To create Cognigy options right-click and choose 'Create\\Cognigy\\Cognigy Options'";

        public CognigyNullOptionsException() : base(NullOptionsMessage) { }
    }
}
