using UnityEditor;
using UnityEngine;

public enum SpeechToTextProvider
{
    Windows
}

public class SpeechToTextWindow : OptionsWindow
{
    [SerializeField]
    private GUISkin guiSkin;
    private SpeechToTextProvider speechToTextProvider;

    [MenuItem("Window/Cognigy/Speech To Text")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(SpeechToTextWindow));
    }

    private void Awake()
    {
        serviceType = "STT";
        this.minSize = new Vector2(400, 400);
    }

    public override OptionsDrawer SetDrawer()
    {
        switch (speechToTextProvider)
        {
            case SpeechToTextProvider.Windows:
                if (currentDrawer == null || currentDrawer.GetType() != typeof(WindowsSTTOptionsDrawer))
                {
                    currentDrawer = CreateInstance<WindowsSTTOptionsDrawer>();
                    currentDrawer.Initialize();
                    currentDrawer.SetWindow(this);
                }

                return currentDrawer;

            default:
                return null;
        }
    }

    public override void AttachOptions(ServiceOptions serviceOptions)
    {
        string path = "Assets";

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + serviceType + "_" + serviceOptions.ServiceName + ".asset");

        AssetDatabase.CreateAsset(serviceOptions, assetPathAndName);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        SpeechToText sttComponent;

        if ((sttComponent = Selection.activeTransform.gameObject.GetComponent<SpeechToText>()) == null)
            sttComponent = Selection.activeTransform.gameObject.AddComponent<SpeechToText>();

        sttComponent.speechToTextOptions = (SpeechToTextOptions)serviceOptions;

        EditorUtility.DisplayDialog("Speech To Text", serviceOptions.ServiceName + " attached to character(s):\n" + Selection.activeTransform.gameObject.name, "Ok");
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
