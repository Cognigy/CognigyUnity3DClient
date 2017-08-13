using System;
using UnityEngine;

public class SpeechToTextService : MonoBehaviour
{
    public event EventHandler<SpeechToTextResultEventArgs> STTResult;

    public virtual void Initialize(SpeechToTextOptions speechToTextOptions) { }

    public virtual void DisableSpeechToText()
    {
        throw new NotImplementedException();
    }

    public virtual void EnableSpeechToText()
    {
        throw new NotImplementedException();
    }

    public virtual void ProcessAudioToText(AudioClip audio)
    {
        throw new NotImplementedException();
    }

    public virtual void OnSTTResult(SpeechToTextResultEventArgs args)
    {
        EventHandler<SpeechToTextResultEventArgs> handler = STTResult;
        if (handler != null)
            handler(this, args);
    }
}
