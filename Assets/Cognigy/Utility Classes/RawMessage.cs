using System;

namespace Cognigy
{
    [Serializable]
    public class RawMessage<T>
    {

        public string text { get; private set; }
        public T data { get; private set; }

        public RawMessage(string text, T data)
        {
            this.text = text;
            this.data = data;
        }
    }
}
