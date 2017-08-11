using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class WindowsSTTOptionsDrawer : OptionsDrawer
{
    WindowsSTTOptions windowsSTTOptions;

    private string autoSilenceTimeoutSecondsTemp = string.Empty;

    public override void Initialize()
    {
        windowsSTTOptions = CreateInstance<WindowsSTTOptions>();
        windowsSTTOptions.ServiceName = "Windows";
    }

    public override void DrawOptions()
    {
#if UNITY_EDITOR_WIN
        GUILayout.BeginVertical("box");
        GUILayout.Label("The Windows Dictation Recognizer uses the default system language for it's speech recognition and can only be run on Windows 10.\n\n" +
            "Your System Language is: " + Application.systemLanguage.ToString());
        GUILayout.EndVertical();

        GUILayout.Space(20);

        GUILayout.Label("Confidence Level");
        windowsSTTOptions.ConfidenceLevel = (ConfidenceLevel)EditorGUILayout.EnumPopup((ConfidenceLevel)windowsSTTOptions.ConfidenceLevel, GUI.skin.GetStyle("customEnum"));

        GUILayout.Space(20);

        GUILayout.Label(new GUIContent("Auto Silence Timeout", "The time length in seconds before dictation recognizer ends due lack if audio input"));
        autoSilenceTimeoutSecondsTemp = GUILayout.TextField(autoSilenceTimeoutSecondsTemp);
        autoSilenceTimeoutSecondsTemp = Regex.Replace(autoSilenceTimeoutSecondsTemp, "[^0-9]", "");
        float.TryParse(autoSilenceTimeoutSecondsTemp, out windowsSTTOptions.AutoSilenceTimeoutSeconds);

#else
        GUILayout.Label("Service not supported on your platform");

#endif
    }

    public override ServiceOptions GetOptions()
    {
        return this.windowsSTTOptions;
    }
}
