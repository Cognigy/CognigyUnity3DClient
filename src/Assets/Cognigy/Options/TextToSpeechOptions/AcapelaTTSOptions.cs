using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Acapela_TTS_Options", menuName = "Text To Speech/Acapela TTS Options", order = 1)]
[Serializable]
public class AcapelaTTSOptions : TextToSpeechOptions
{
    [SerializeField]
    public string VaasUrl;
    [SerializeField]
    public string Login;
    [SerializeField]
    public string Password;
    [SerializeField]
    public string App;

    public override Type GetServiceType()
    {
        return this.ServiceType = typeof(AcapelaTTS);
    }
}
