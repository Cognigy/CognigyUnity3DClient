using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class WindowsSTTOptionsDrawer : OptionsDrawer
{
    WindowsSTTOptions windowsSTTOptions;

    private string autoSilenceTimeoutSecondsTemp = string.Empty;

    public override void DrawOptions<TOptions>(TOptions serviceOptions)
    {
        windowsSTTOptions = serviceOptions as WindowsSTTOptions;

#if UNITY_EDITOR_WIN
        DrawExplanationBox();

        GUILayout.Space(20);

        DrawConfidenceEnum();

        GUILayout.Space(20);

        DrawAutoSilenceField();
#else
        GUILayout.Label("Service not supported on your platform");

#endif
    }

    #region GuiElements
    private void DrawExplanationBox()
    {
        GUILayout.BeginVertical("box");
        GUILayout.Label("The Windows Dictation Recognizer uses the default system language for it's speech recognition and can only be run on Windows 10.\n\n" +
            "Your System Language is: " + Application.systemLanguage.ToString());
        GUILayout.EndVertical();
    }

    private void DrawConfidenceEnum()
    {
        GUILayout.Label("Confidence Level");
        windowsSTTOptions.ConfidenceLevel = (ConfidenceLevel)EditorGUILayout.EnumPopup((ConfidenceLevel)windowsSTTOptions.ConfidenceLevel, GUI.skin.GetStyle("customEnum"));
    }

    private void DrawAutoSilenceField()
    {
        GUILayout.Label(new GUIContent("Auto Silence Timeout", "The time length in seconds before dictation recognizer ends due lack if audio input"));
        autoSilenceTimeoutSecondsTemp = GUILayout.TextField(autoSilenceTimeoutSecondsTemp);
        autoSilenceTimeoutSecondsTemp = Regex.Replace(autoSilenceTimeoutSecondsTemp, "[^0-9]", "");
        float.TryParse(autoSilenceTimeoutSecondsTemp, out windowsSTTOptions.AutoSilenceTimeoutSeconds);
    }
    #endregion
}
