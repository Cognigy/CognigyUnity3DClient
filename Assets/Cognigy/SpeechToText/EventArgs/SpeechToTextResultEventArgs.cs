using System;

public class SpeechToTextResultEventArgs : EventArgs
{
    public string STTResult { get; private set; }

    public SpeechToTextResultEventArgs(string sttResult)
    { this.STTResult = sttResult; }
}
