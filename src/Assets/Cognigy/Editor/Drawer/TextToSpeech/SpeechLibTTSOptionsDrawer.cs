using SpeechLib;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class SpeechLibTTSOptionsDrawer : OptionsDrawer
{
    private SpeechLibTTSOptions serviceOptions;

    private string rateTemp = string.Empty;
    private string volumeTemp = string.Empty;

    private string[] voices;
    private int selection = 0;

    public override void DrawOptions<TOptions>(TOptions serviceOptions)
    {
        this.serviceOptions = serviceOptions as SpeechLibTTSOptions;

        DrawExplanationLabel();

        DrawVoicePopup();

        GUILayout.Space(20);

        DrawRateField();

        DrawVolumeField();
    }

    #region GuiElements
    private void DrawVoicePopup()
    {
        SpObjectTokenCategory tokenCategory = new SpObjectTokenCategory();
        tokenCategory.SetId(SpeechLib.SpeechStringConstants.SpeechCategoryVoices, false);
        ISpeechObjectTokens tokens = tokenCategory.EnumerateTokens(null, null);

        voices = new string[tokens.Count];

        int n = 0;
        foreach (SpObjectToken item in tokens)
        {
            voices[n] = item.GetDescription(0);
            n++;
        }

        selection = EditorGUILayout.Popup(selection, voices, GUI.skin.GetStyle("customEnum"));
        serviceOptions.Voice = selection;
    }

    private void DrawRateField()
    {
        GUILayout.Label("Rate");
        rateTemp = serviceOptions.Rate.ToString();
        rateTemp = GUILayout.TextField(rateTemp);
        rateTemp = Regex.Replace(rateTemp, "[^0-9]", "");
        int.TryParse(rateTemp, out serviceOptions.Rate);
    }

    private void DrawVolumeField()
    {
        GUILayout.Label("Volume");
        volumeTemp = serviceOptions.Volume.ToString();
        volumeTemp = GUILayout.TextField(volumeTemp);
        volumeTemp = Regex.Replace(volumeTemp, "[^0-9]", "");
        int.TryParse(volumeTemp, out serviceOptions.Volume);
    }

    private void DrawExplanationLabel()
    {
        GUILayout.Label("The SpeechLib Text to Speech services uses the installed TTS voices and works offline.");
    }
    #endregion
}
