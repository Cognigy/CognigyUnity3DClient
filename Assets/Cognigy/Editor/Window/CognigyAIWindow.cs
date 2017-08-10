using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Cognigy
{
    public class CognigyAIWindow : EditorWindow
    {
        [SerializeField]
        private GUISkin guiSkin;
        [SerializeField]
        private Texture2D cognigyLogo;

        private AIOptions aiOptions;

        private string versionTemp = string.Empty;
        private string intervalTemp = string.Empty;
        private string timeoutTemp = string.Empty;

        private bool showConnectionFields = false;
        private bool showFlowFields = false;
        private bool showAdditionFields = false;

        private bool hasSelection;

        [MenuItem("Window/Cognigy/Cognigy AI")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(CognigyAIWindow));
        }

        private void Awake()
        {
            this.minSize = new Vector2(400, 830);
            aiOptions = CreateInstance<AIOptions>();
        }

        void OnGUI()
        {
            GUI.skin = guiSkin;
            DrawOptions();
        }

        private void DrawOptions()
        {
            GUILayout.Space(15);

            DrawLogo();

            GUILayout.Space(10);

            if (GUILayout.Button("Connection"))
                showConnectionFields = !showConnectionFields;

            if (showConnectionFields)
            {
                GUILayout.BeginVertical("box");

                DrawServerUrlField();

                DrawTimeoutField();

                DrawApikeyField();

                DrawTokenField();

                GUILayout.EndVertical();
            }

            GUILayout.Space(20);

            if (GUILayout.Button("Flow Settings"))
                showFlowFields = !showFlowFields;

            if (showFlowFields)
            {
                GUILayout.BeginVertical("box");

                DrawUserField();

                DrawFlowField();

                DrawVersionField();

                DrawLanguageEnum();

                GUILayout.Space(10);

                GUILayout.EndVertical();
            }

            GUILayout.Space(20);

            if (GUILayout.Button("Additional Settings"))
                showAdditionFields = !showAdditionFields;

            if (showAdditionFields)
            {
                GUILayout.BeginVertical("box");
                DrawReconnectionToggle();

                GUILayout.Space(5);

                DrawResetStateToggle();

                GUILayout.Space(5);

                DrawResetContextToggle();

                GUILayout.Space(5);

                DrawChannelField();

                DrawIntervalField();

                DrawPassthroughIpField();

                GUILayout.EndVertical();
            }

            GUILayout.Space(20);

            if (Selection.activeTransform != null)
                hasSelection = true;
            else
                hasSelection = false;

            GUILayout.BeginHorizontal();
            DrawCreateOptionsButton();

            using (new EditorGUI.DisabledScope(hasSelection == false))
            {
                DrawAttachButton();
            }
            GUILayout.EndHorizontal();

        }

        //GUI Elements

        private void DrawSeperator()
        {
            GUILayout.Space(8);

            GUILayout.Box("", guiSkin.GetStyle("seperator"), new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(2) });

            GUILayout.Space(8);
        }

        private void DrawLogo()
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(cognigyLogo);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        private void DrawServerUrlField()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Cognigy AI Server URL");
            GUILayout.Label(new GUIContent("*", "required"), GUI.skin.GetStyle("required"));
            GUILayout.EndHorizontal();
            aiOptions.AIServerUrl = GUILayout.TextField(aiOptions.AIServerUrl);
        }

        private void DrawTimeoutField()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Timeout");
            GUILayout.FlexibleSpace();
            timeoutTemp = GUILayout.TextField(timeoutTemp, GUILayout.Width(50));
            timeoutTemp = Regex.Replace(timeoutTemp, "[^0-9]", "");
            int.TryParse(timeoutTemp, out aiOptions.MillisecondsTimeout);
            GUILayout.Space(5);
            GUI.skin.label.alignment = TextAnchor.UpperRight;
            GUILayout.Label("ms");
            GUI.skin.label.alignment = TextAnchor.UpperLeft;
            GUILayout.EndHorizontal();
        }

        private void DrawApikeyField()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("API Key");
            GUILayout.Label(new GUIContent("*", "required"), GUI.skin.GetStyle("required"));
            GUILayout.EndHorizontal();
            aiOptions.APIKey = GUILayout.TextField(aiOptions.APIKey);
        }

        private void DrawTokenField()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Token");
            GUILayout.Label(new GUIContent("Optional", "Can be empty"), GUI.skin.GetStyle("optional"));
            GUILayout.EndHorizontal();
            aiOptions.Token = GUILayout.TextField(aiOptions.Token);
        }

        private void DrawUserField()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("User");
            GUILayout.Label(new GUIContent("*", "required"), GUI.skin.GetStyle("required"));
            GUILayout.EndHorizontal();
            aiOptions.User = GUILayout.TextField(aiOptions.User);
        }

        private void DrawFlowField()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Flow", "The name of the flow"));
            GUILayout.Label(new GUIContent("*", "required"), GUI.skin.GetStyle("required"));
            GUILayout.EndHorizontal();
            aiOptions.Flow = GUILayout.TextField(aiOptions.Flow);
        }

        private void DrawVersionField()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Version");
            GUILayout.Label(new GUIContent("Optional", "Can be empty"), GUI.skin.GetStyle("optional"));
            GUILayout.EndHorizontal();
            versionTemp = GUILayout.TextField(versionTemp);
            versionTemp = Regex.Replace(versionTemp, "[^0-9]", "");
            int.TryParse(versionTemp, out aiOptions.Version);
        }

        private void DrawLanguageEnum()
        {
            GUILayout.Label("Language");
            aiOptions.Language = (AILanguage)EditorGUILayout.EnumPopup(aiOptions.Language, GUI.skin.GetStyle("customEnum"));
        }

        private void DrawReconnectionToggle()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Reconnection");
            GUILayout.FlexibleSpace();
            aiOptions.Reconnection = GUILayout.Toggle(aiOptions.Reconnection, "", this.guiSkin.customStyles[0]);
            GUILayout.EndHorizontal();
        }

        private void DrawResetStateToggle()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Reset State", "If 'true': resets the state of the flow to default"));
            GUILayout.FlexibleSpace();
            aiOptions.ResetState = GUILayout.Toggle(aiOptions.ResetState, "", this.guiSkin.customStyles[0]);
            GUILayout.EndHorizontal();
        }

        private void DrawResetContextToggle()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Reset Context", "If 'true': resets the context of the flow to default"));
            GUILayout.FlexibleSpace();
            aiOptions.ResetContext = GUILayout.Toggle(aiOptions.ResetContext, "", this.guiSkin.customStyles[0]);
            GUILayout.EndHorizontal();
        }

        private void DrawChannelField()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Channel");
            GUILayout.Label(new GUIContent("Optional", "Can be empty"), GUI.skin.GetStyle("optional"));
            GUILayout.EndHorizontal();
            aiOptions.Channel = GUILayout.TextField(aiOptions.Channel);
        }

        private void DrawIntervalField()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Interval");
            GUILayout.Label(new GUIContent("Optional", "Can be empty"), GUI.skin.GetStyle("optional"));
            GUILayout.EndHorizontal();
            GUI.skin.textField.alignment = TextAnchor.UpperRight;
            intervalTemp = GUILayout.TextField(intervalTemp);
            GUI.skin.textField.alignment = TextAnchor.UpperLeft;
            intervalTemp = Regex.Replace(intervalTemp, "[^0-9]", "");
            int.TryParse(intervalTemp, out aiOptions.Interval);
        }

        private void DrawPassthroughIpField()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Passthrough IP");
            GUILayout.Label(new GUIContent("Optional", "Can be empty"), GUI.skin.GetStyle("optional"));
            GUILayout.EndHorizontal();
            aiOptions.PassthroughIP = GUILayout.TextField(aiOptions.PassthroughIP);
        }

        private void DrawAttachButton()
        {
            if (GUILayout.Button("Attach AI"))
            {
                AttachOptions();

                this.Close();
            }
        }

        private void DrawCreateOptionsButton()
        {
            if (GUILayout.Button("Create AI Options"))
            {
                CreateOptions();

                this.Close();
            }
        }

        private void AttachOptions()
        {
            string path = "Assets";
            string assetName;

            if (Selection.activeTransform != null)
                assetName = Selection.activeTransform.gameObject.name;
            else
            {
                if (!string.IsNullOrEmpty(aiOptions.Flow))
                    assetName = aiOptions.Flow;
                else
                    assetName = "Options";
            }

            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + "AI_" + assetName + ".asset");

            AssetDatabase.CreateAsset(aiOptions, assetPathAndName);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            CognigyAI cognigyAI;

            if ((cognigyAI = Selection.activeTransform.gameObject.GetComponent<CognigyAI>()) == null)
                cognigyAI = Selection.activeTransform.gameObject.AddComponent<CognigyAI>();

            cognigyAI.aiOptions = aiOptions;

            EditorUtility.DisplayDialog("Cognigy AI", "Cognigy AI attached to character(s):\n" + Selection.activeTransform.gameObject.name, "Ok");
        }

        private void CreateOptions()
        {
            string path = "Assets";
            string assetName;

            if (Selection.activeTransform != null)
                assetName = Selection.activeTransform.gameObject.name;
            else
            {
                if (!string.IsNullOrEmpty(aiOptions.Flow))
                    assetName = aiOptions.Flow;
                else
                    assetName = "Options";
            }

            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + "AI_" + assetName + ".asset");

            AssetDatabase.CreateAsset(aiOptions, assetPathAndName);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();


            EditorUtility.DisplayDialog("Cognigy AI", "Options created at:\n" + assetPathAndName, "Ok");
        }

    }
}
