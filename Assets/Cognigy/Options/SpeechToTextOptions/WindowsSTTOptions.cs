using System;
using UnityEngine;
using UnityEngine.Windows.Speech;

[Serializable]
[CreateAssetMenu(fileName = "Windows_STT_Options", menuName = "Speech To Text/Windows STT Options", order = 1)]
public class WindowsSTTOptions : SpeechToTextOptions
{
    [SerializeField]
    public ConfidenceLevel ConfidenceLevel = ConfidenceLevel.Low;

    [SerializeField]
    public float AutoSilenceTimeoutSeconds;

    public override Type GetServiceType()
    {
        return this.ServiceType = typeof(WindowsSTT);
    }
}
