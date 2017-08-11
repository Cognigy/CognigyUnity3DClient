using Newtonsoft.Json;
using System;

namespace Cognigy
{
    [Serializable]
    class InitializationParameters
    {
        public string flowId;
        public string language;
        public float? version;
        public string passthroughIP;
        public bool? resetState;
        public bool? resetContext;

        public InitializationParameters(string flowId, string language, float? version, string passthroughIP, bool? resetState, bool? resetContext)
        {
            this.flowId = flowId;
            this.language = language;
            this.version = version;
            this.passthroughIP = passthroughIP;
            this.resetState = resetState;
            this.resetContext = resetContext;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
