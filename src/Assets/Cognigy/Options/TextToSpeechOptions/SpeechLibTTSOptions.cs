using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SpeechLib_TTS_Options", menuName = "Text To Speech/SpeechLib TTS Options", order = 2)]
public class SpeechLibTTSOptions : TextToSpeechOptions
{
    [SerializeField]
    public int Volume;

    public override Type GetServiceType()
    {
        return this.ServiceType = typeof(SpeechLibTTS);
    }
}
