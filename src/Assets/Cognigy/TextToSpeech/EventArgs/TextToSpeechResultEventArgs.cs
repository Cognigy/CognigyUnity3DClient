using System;
using UnityEngine;

public class TextToSpeechResultEventArgs : EventArgs
{
    public AudioClip TTSResult { get; set; }

    public TextToSpeechResultEventArgs(AudioClip ttsResult)
    {
        this.TTSResult = ttsResult;
    }
}
