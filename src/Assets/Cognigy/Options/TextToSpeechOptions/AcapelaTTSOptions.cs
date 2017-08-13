using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Acapela_TTS_Options", menuName = "Text To Speech/Acapela TTS Options", order = 1)]
[Serializable]
public class AcapelaTTSOptions : TextToSpeechOptions
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
            return "Acapela";
        }
    }

    public override Type ServiceImplementation
    {
        get
        {
            return typeof(AcapelaTTS);
        }
    }

    [SerializeField]
    public string VaasUrl;
    [SerializeField]
    public string Login;
    [SerializeField]
    public string Password;
    [SerializeField]
    public string App;
}
