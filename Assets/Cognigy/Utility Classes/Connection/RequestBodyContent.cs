using Newtonsoft.Json;
using System;

namespace Cognigy
{
    [Serializable]
    class RequestBodyContent
    {
        [JsonProperty]
        private string user;
        [JsonProperty]
        private string apikey;
        [JsonProperty]
        private string channel;

        public RequestBodyContent(string user, string apikey, string channel)
        {
            this.user = user;
            this.apikey = apikey;
            this.channel = channel;

        }
    }
}
