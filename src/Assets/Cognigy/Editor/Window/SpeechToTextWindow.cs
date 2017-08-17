using UnityEditor;
using UnityEngine;

public enum SpeechToTextProvider
{
    Windows
}

public class SpeechToTextWindow : OptionsWindow<SpeechToText>
{
    private SpeechToTextProvider speechToTextProvider;

    [MenuItem("Window/Cognigy/Speech To Text")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(SpeechToTextWindow));
    }

    public override void WindowSetup()
    {
        this.minSize = new Vector2(400, 400);
    }


    public override void SetDrawer()
    {
        switch (speechToTextProvider)
        {
            case SpeechToTextProvider.Windows:
                if (currentDrawer == null || currentDrawer.GetType() != typeof(WindowsSTTOptionsDrawer))
                {
                    currentDrawer = CreateInstance<WindowsSTTOptionsDrawer>();
                    currentOptions = CreateInstance<WindowsSTTOptions>();
                }
                break;

            default:
                currentDrawer = CreateInstance<DefaultDrawer>();
                currentOptions = CreateInstance<ServiceOptions>();
                break;
        }
    }

    public override void DrawHeader()
    {
        GUI.skin = guiSkin;
        GUILayout.BeginVertical("", guiSkin.GetStyle("HeaderBackground"));
        GUILayout.Label("Speech To Text", guiSkin.GetStyle("Header"));
        GUILayout.EndVertical();
    }

    public override void DrawSelector()
    {
        GUILayout.Space(10);
        GUILayout.Label("Select Provider");
        speechToTextProvider = (SpeechToTextProvider)EditorGUILayout.EnumPopup(speechToTextProvider, GUI.skin.GetStyle("customEnum"));
        GUILayout.Space(10);
    }

}
