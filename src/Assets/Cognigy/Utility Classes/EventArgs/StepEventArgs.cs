using System;

namespace Cognigy
{
    public class StepEventArgs : EventArgs
    {
        public Step Step { get; private set; }

        public StepEventArgs(Step step)
        { this.Step = step; }
    }
}
