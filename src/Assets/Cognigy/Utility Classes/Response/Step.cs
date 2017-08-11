using Newtonsoft.Json.Linq;

namespace Cognigy
{
    public class Step
    {
        public string id { get; set; }
        public JContainer context { get; set; }
    }
}
