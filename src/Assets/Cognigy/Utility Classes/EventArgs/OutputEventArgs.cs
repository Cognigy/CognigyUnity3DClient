using System;

namespace Cognigy
{
    public class OutputEventArgs : EventArgs
    {
        public Output Output { get; private set; }

        public OutputEventArgs(Output output)
        { this.Output = output; }
    }
}
