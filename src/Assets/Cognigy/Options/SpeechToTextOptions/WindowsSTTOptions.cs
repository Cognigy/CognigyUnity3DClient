using System;
using UnityEngine;
using UnityEngine.Windows.Speech;

[Serializable]
[CreateAssetMenu(fileName = "Windows_STT_Options", menuName = "Speech To Text/Windows STT Options", order = 1)]
public class WindowsSTTOptions : SpeechToTextOptions
{
    public override ServiceType ServiceType
    {
        get
        {
            return ServiceType.Speech_To_Text;
        }
    }

    public override string ServiceName
    {
        get
        {
            return "Windows";
        }
    }

    public override Type ServiceImplementation
    {
        get
        {
            return typeof(WindowsSTT);
        }
    }

    [SerializeField]
    public ConfidenceLevel ConfidenceLevel = ConfidenceLevel.Low;

    [SerializeField]
    public float AutoSilenceTimeoutSeconds;
}
