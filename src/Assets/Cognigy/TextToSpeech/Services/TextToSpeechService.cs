using System;
using UnityEngine;

public class TextToSpeechService : MonoBehaviour
{
    public event EventHandler<TextToSpeechResultEventArgs> TTSResult;

    public virtual void Initialize(TextToSpeechOptions textToSpeechOptions)
    {
    }

    public virtual void ProcessTextToAudio(string input)
    {
    }

    protected virtual void OnTTSResult(TextToSpeechResultEventArgs args)
    {
        EventHandler<TextToSpeechResultEventArgs> handler = TTSResult;
        if (handler != null)
            handler(this, args);
    }
}
