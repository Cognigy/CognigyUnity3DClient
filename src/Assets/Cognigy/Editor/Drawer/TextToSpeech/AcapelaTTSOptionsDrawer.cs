using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class AcapelaTTSOptionsDrawer : OptionsDrawer
{
    private AcapelaTTSOptions serviceOptions;
    private string rateTemp = string.Empty;

    public override void DrawOptions<TOptions>(TOptions serviceOptions)
    {
        this.serviceOptions = serviceOptions as AcapelaTTSOptions;

        DrawVaasURLField();

        DrawLoginField();

        DrawPasswordField();

        DrawAppField();

        DrawVoiceEnum();

        GUILayout.Space(10);

        DrawRateField();
    }

    #region GuiElements
    private void DrawVaasURLField()
    {
        GUILayout.Label("VAAS Server URL");
        serviceOptions.VaasUrl = GUILayout.TextField(serviceOptions.VaasUrl);
    }

    private void DrawLoginField()
    {
        GUILayout.Label("Login");
        serviceOptions.Login = GUILayout.TextField(serviceOptions.Login);
    }

    private void DrawPasswordField()
    {
        GUILayout.Label("Password");
        serviceOptions.Password = GUILayout.TextField(serviceOptions.Password);
    }

    private void DrawAppField()
    {
        GUILayout.Label("App");
        serviceOptions.App = GUILayout.TextField(serviceOptions.App);
    }

    private void DrawVoiceEnum()
    {
        GUILayout.Label("Voice");
        serviceOptions.Voice = (int)((AcapelaVoice)EditorGUILayout.EnumPopup((AcapelaVoice)serviceOptions.Voice, GUI.skin.GetStyle("customEnum")));
    }

    private void DrawRateField()
    {
        GUILayout.Label("Rate");
        rateTemp = serviceOptions.Rate.ToString();
        rateTemp = GUILayout.TextField(rateTemp);
        rateTemp = Regex.Replace(rateTemp, "[^0-9]", "");
        int.TryParse(rateTemp, out serviceOptions.Rate);
    }
    #endregion
}
