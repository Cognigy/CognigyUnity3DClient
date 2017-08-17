using System;
using UnityEngine;

public abstract class TextToSpeechService : MonoBehaviour
{
    public event EventHandler<TextToSpeechResultEventArgs> TTSResult;

    public abstract void Initialize(TextToSpeechOptions textToSpeechOptions);

    public abstract void ProcessTextToAudio(string input);

    protected virtual void OnTTSResult(TextToSpeechResultEventArgs args)
    {
        EventHandler<TextToSpeechResultEventArgs> handler = TTSResult;
        if (handler != null)
            handler(this, args);
    }
}
