using Newtonsoft.Json.Linq;
using System;

namespace Cognigy
{
    public class FlowOutputEventArgs : EventArgs
    {
        public FlowOutput FlowOutput { get; private set; }

        public FlowOutputEventArgs(FlowOutput flowOutput)
        { this.FlowOutput = flowOutput; }
    }
}
