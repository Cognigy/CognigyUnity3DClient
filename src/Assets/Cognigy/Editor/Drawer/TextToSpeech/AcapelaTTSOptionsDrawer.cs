using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class AcapelaTTSOptionsDrawer : OptionsDrawer
{
    private AcapelaTTSOptions acapelaOptions;

    private string rateTemp = string.Empty;

    public override void Initialize()
    {
        acapelaOptions = CreateInstance<AcapelaTTSOptions>();
        acapelaOptions.ServiceName = "Acapela";
    }

    public override void DrawOptions()
    {
        GUILayout.Label("VAAS Server URL");
        acapelaOptions.VaasUrl = GUILayout.TextField(acapelaOptions.VaasUrl);

        GUILayout.Label("Login");
        acapelaOptions.Login = GUILayout.TextField(acapelaOptions.Login);

        GUILayout.Label("Password");
        acapelaOptions.Password = GUILayout.TextField(acapelaOptions.Password);

        GUILayout.Label("App");
        acapelaOptions.App = GUILayout.TextField(acapelaOptions.App);

        GUILayout.Label("Voice");
        acapelaOptions.Voice = (int)((AcapelaVoice)EditorGUILayout.EnumPopup((AcapelaVoice)acapelaOptions.Voice, GUI.skin.GetStyle("customEnum")));

        GUILayout.Space(10);

        GUILayout.Label("Rate");
        rateTemp = GUILayout.TextField(rateTemp);
        rateTemp = Regex.Replace(rateTemp, "[^0-9]", "");
        int.TryParse(rateTemp, out acapelaOptions.Rate);
    }

    public override ServiceOptions GetOptions()
    {
        return this.acapelaOptions;
    }
}
