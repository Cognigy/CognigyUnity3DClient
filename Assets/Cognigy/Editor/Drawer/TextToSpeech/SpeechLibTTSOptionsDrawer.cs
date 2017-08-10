using SpeechLib;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class SpeechLibTTSOptionsDrawer : OptionsDrawer
{
    private SpeechLibTTSOptions speechLibOptions;

    private string rateTemp = string.Empty;
    private string volumeTemp = string.Empty;

    private string[] voices;
    private int selection = 0;

    public override void Initialize()
    {
        speechLibOptions = CreateInstance<SpeechLibTTSOptions>();
        speechLibOptions.ServiceName = "SpeechLib";
    }

    public override void DrawOptions()
    {
        DrawExplanationLabel();

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
        speechLibOptions.Voice = selection;

        GUILayout.Space(20);

        GUILayout.Label("Rate");
        rateTemp = GUILayout.TextField(rateTemp);
        rateTemp = Regex.Replace(rateTemp, "[^0-9]", "");
        int.TryParse(rateTemp, out speechLibOptions.Rate);

        GUILayout.Label("Volume");
        volumeTemp = GUILayout.TextField(volumeTemp);
        volumeTemp = Regex.Replace(volumeTemp, "[^0-9]", "");
        int.TryParse(volumeTemp, out speechLibOptions.Volume);
    }

    public override ServiceOptions GetOptions()
    {
        return this.speechLibOptions;
    }

    private void DrawExplanationLabel()
    {
        GUILayout.Label("The SpeechLib Text to Speech services uses the installed TTS voices and works offline.");
    }
}
