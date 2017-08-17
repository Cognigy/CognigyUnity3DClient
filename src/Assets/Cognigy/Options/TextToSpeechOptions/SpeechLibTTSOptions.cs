using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SpeechLib_TTS_Options", menuName = "Text To Speech/SpeechLib TTS Options", order = 2)]
public class SpeechLibTTSOptions : TextToSpeechOptions
{
    public override ServiceType ServiceType
    {
        get
        {
            return ServiceType.Text_To_Speech;
        }
    }

    public override string ServiceName
    {
        get
        {
            return "SpeechLib";
        }
    }

    public override Type ServiceImplementation
    {
        get
        {
            return typeof(SpeechLibTTS);
        }
    }

    [SerializeField]
    public int Volume = 100;
}
