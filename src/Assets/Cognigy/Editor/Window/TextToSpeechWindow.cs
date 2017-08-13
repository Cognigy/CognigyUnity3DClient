using UnityEditor;
using UnityEngine;

public enum TextToSpeechProvider
{
    Acapela,
    SpeechLib
}

public class TextToSpeechWindow : OptionsWindow<TextToSpeech>
{
    private TextToSpeechProvider textToSpeechProvider;

    [MenuItem("Window/Cognigy/Text To Speech")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(TextToSpeechWindow));
    }

    public override void WindowSetup()
    {
        typesAndServices.Add(typeof(AcapelaTTS), null);
        typesAndServices.Add(typeof(SpeechLibTTS), null);
        this.minSize = new Vector2(400, 580);
    }

    public override void GuiSkinSetup()
    {
        GUI.skin = guiSkin;
    }

    public override void DrawHeader()
    {
        GUILayout.BeginVertical("", guiSkin.GetStyle("HeaderBackground"));
        GUILayout.Label("Text to Speech", guiSkin.GetStyle("Header"));
        GUILayout.EndVertical();
    }

    public override void DrawSelector()
    {
        GUILayout.Space(10);
        GUILayout.Label("Select Provider");
        textToSpeechProvider = (TextToSpeechProvider)EditorGUILayout.EnumPopup(textToSpeechProvider, GUI.skin.GetStyle("customEnum"));
        GUILayout.Space(30);
    }

    public override void SetDrawer()
    {
        switch (textToSpeechProvider)
        {
            case TextToSpeechProvider.Acapela:
                if (currentDrawer == null || currentDrawer.GetType() != typeof(AcapelaTTSOptionsDrawer))
                {
                    if (typesAndServices[typeof(AcapelaTTS)] == null)
                    {
                        currentOptions = CreateInstance<AcapelaTTSOptions>();
                        typesAndServices[typeof(AcapelaTTS)] = currentOptions;
                    }
                    else
                    {
                        currentOptions = typesAndServices[typeof(AcapelaTTS)];
                    }

                    currentDrawer = CreateInstance<AcapelaTTSOptionsDrawer>();
                }
                break;

            case TextToSpeechProvider.SpeechLib:
                if (currentDrawer == null || currentDrawer.GetType() != typeof(SpeechLibTTSOptionsDrawer))
                {
                    if (typesAndServices[typeof(SpeechLibTTS)] == null)
                    {
                        currentOptions = CreateInstance<SpeechLibTTSOptions>();
                        typesAndServices[typeof(SpeechLibTTS)] = currentOptions;
                    }
                    else
                    {
                        currentOptions = typesAndServices[typeof(SpeechLibTTS)];
                    }

                    currentDrawer = CreateInstance<SpeechLibTTSOptionsDrawer>();
                }
                break;

            default:
                currentDrawer = CreateInstance<DefaultDrawer>();
                currentOptions = CreateInstance<ServiceOptions>();
                break;
        }
    }
}
