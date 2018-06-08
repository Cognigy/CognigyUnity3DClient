using System;

namespace Cognigy
{
    [Serializable]
    public class CognigyInput<T>
    {
        public string URLToken { get; private set; }
        public string userId { get; private set; }
        public string sessionId { get; private set; }
        public string text { get; private set; }
        public T data { get; private set; }

        private const string source = "device";

        public CognigyInput(string URLToken, string userId, string sessionId, string text, T data)
        {
            this.URLToken = URLToken;
            this.userId = userId;
            this.sessionId = sessionId;
            this.text = text;
            this.data = data;
        }
    }
}
