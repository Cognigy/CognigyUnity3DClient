using UnityEditor;
using UnityEngine;

public enum TextToSpeechProvider
{
    Acapela,
    SpeechLib
}

public class TextToSpeechWindow : OptionsWindow
{
    [SerializeField]
    private GUISkin guiSkin;
    private TextToSpeechProvider textToSpeechProvider;

    [MenuItem("Window/Cognigy/Text To Speech")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(TextToSpeechWindow));
    }

    public override void AttachOptions(ServiceOptions serviceOptions)
    {
        string path = "Assets";

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + serviceType + "_" + serviceOptions.ServiceName + ".asset");

        AssetDatabase.CreateAsset(serviceOptions, assetPathAndName);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        if (Selection.activeTransform.gameObject.GetComponent<AudioSource>() == null)
            Selection.activeTransform.gameObject.AddComponent<AudioSource>();

        TextToSpeech ttsComponent;

        if ((ttsComponent = Selection.activeTransform.gameObject.GetComponent<TextToSpeech>()) == null)
            ttsComponent = Selection.activeTransform.gameObject.AddComponent<TextToSpeech>();

        ttsComponent.textToSpeechOptions = (TextToSpeechOptions)serviceOptions;

        EditorUtility.DisplayDialog("Text to Speech", serviceOptions.ServiceName + " attached to character(s):\n" + Selection.activeTransform.gameObject.name, "Ok");
    }

    private void Awake()
    {
        serviceType = "TTS";
        this.minSize = new Vector2(400, 550);
    }

    public override OptionsDrawer SetDrawer()
    {
        switch (textToSpeechProvider)
        {
            case TextToSpeechProvider.Acapela:
                if (currentDrawer == null || currentDrawer.GetType() != typeof(AcapelaTTSOptionsDrawer))
                {
                    currentDrawer = CreateInstance<AcapelaTTSOptionsDrawer>();
                    currentDrawer.Initialize();
                    currentDrawer.SetWindow(this);
                }

                return currentDrawer;

            case TextToSpeechProvider.SpeechLib:
                if (currentDrawer == null || currentDrawer.GetType() != typeof(SpeechLibTTSOptionsDrawer))
                {
                    currentDrawer = CreateInstance<SpeechLibTTSOptionsDrawer>();
                    currentDrawer.Initialize();
                    currentDrawer.SetWindow(this);
                }

                return currentDrawer;

            default:
                return null;
        }
    }

    public override void DrawHeader()
    {
        GUI.skin = guiSkin;
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
}
