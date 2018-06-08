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

        private SocketEndpointOptions socketEndpointOptions;

        private string intervalTemp = string.Empty;
        private string timeoutTemp = string.Empty;

        private string selectedObject = string.Empty;

        private bool showConnectionFields = true;
        private bool showAdditionFields = false;

        private TextAnchor defaultLabelAlignment;

        private Vector2 scrollPosition;

        private bool hasSelection;

        [MenuItem("Window/Cognigy/COGNIGY.AI")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(CognigyAIWindow));
        }

        private void Awake()
        {
            this.minSize = new Vector2(420, 740);
            socketEndpointOptions = CreateInstance<SocketEndpointOptions>();
        }

        void OnGUI()
        {
            GUI.skin = guiSkin;
            defaultLabelAlignment = GUI.skin.label.alignment;

            if (Selection.activeTransform != null)
            {
                selectedObject = Selection.activeTransform.gameObject.name;
                hasSelection = true;
            }
            else
            {
                selectedObject = "none";
                hasSelection = false;
            }

            DrawOptions();
        }

        private void OnInspectorUpdate()
        {
            Repaint();
        }

        private void DrawOptions()
        {
            GUILayout.Space(15);

            DrawLogo();

            GUILayout.Space(10);

            DrawSelectedObjectLabel();

            OptionsLayout.HorizontalLine(this.guiSkin);

            GUILayout.Space(10);

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            if (GUILayout.Button("Connection"))
                showConnectionFields = !showConnectionFields;

            if (showConnectionFields)
            {
                GUILayout.BeginVertical("box");

                DrawEndpointURLField();

                DrawURLTokenField();

                DrawUserIdField();

                DrawSessionIdField();

                DrawTimeoutField();

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

                DrawPassthroughIpField();

                GUILayout.EndVertical();
            }


            EditorGUILayout.EndScrollView();

            GUILayout.Space(12);

            OptionsLayout.HorizontalLine(this.guiSkin);

            GUILayout.Space(12);


            GUILayout.BeginHorizontal();
            DrawCreateOptionsButton();

            using (new EditorGUI.DisabledScope(hasSelection == false))
            {
                DrawAttachButton();
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(12);
        }

        #region GuiElements

        private void DrawLogo()
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(cognigyLogo);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        private void DrawEndpointURLField()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Endpoint URL");
            GUILayout.Label(new GUIContent("*", "required"), GUI.skin.GetStyle("required"));
            GUILayout.EndHorizontal();
            socketEndpointOptions.EndpointURL = EditorGUILayout.TextField(socketEndpointOptions.EndpointURL, GUI.skin.GetStyle("TextField"));
        }

        private void DrawTimeoutField()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Timeout");
            GUILayout.FlexibleSpace();
            timeoutTemp = socketEndpointOptions.MillisecondsTimeout.ToString();
            timeoutTemp = GUILayout.TextField(timeoutTemp, GUILayout.Width(50));
            timeoutTemp = Regex.Replace(timeoutTemp, "[^0-9]", "");
            int.TryParse(timeoutTemp, out socketEndpointOptions.MillisecondsTimeout);
            GUILayout.Space(5);
            GUI.skin.label.alignment = TextAnchor.UpperRight;
            GUILayout.Label("ms");
            GUI.skin.label.alignment = TextAnchor.UpperLeft;
            GUILayout.EndHorizontal();
        }

        private void DrawURLTokenField()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("URL Token");
            GUILayout.Label(new GUIContent("*", "required"), GUI.skin.GetStyle("required"));
            GUILayout.EndHorizontal();
            socketEndpointOptions.URLToken = EditorGUILayout.TextField(socketEndpointOptions.URLToken, GUI.skin.GetStyle("TextField"));
        }

        private void DrawUserIdField()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("User ID");
            GUILayout.Label(new GUIContent("*", "required"), GUI.skin.GetStyle("required"));
            GUILayout.EndHorizontal();
            socketEndpointOptions.UserID = EditorGUILayout.TextField(socketEndpointOptions.UserID, GUI.skin.GetStyle("TextField"));
        }

        private void DrawSessionIdField()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Session ID");
            GUILayout.Label(new GUIContent("*", "required"), GUI.skin.GetStyle("required"));
            GUILayout.EndHorizontal();
            socketEndpointOptions.SessionID = EditorGUILayout.TextField(socketEndpointOptions.SessionID, GUI.skin.GetStyle("TextField"));
        }
        private void DrawReconnectionToggle()
        {
            GUILayout.BeginHorizontal();
            GUI.skin.label.alignment = TextAnchor.MiddleLeft;
            GUILayout.Label("Reconnection");
            GUI.skin.label.alignment = defaultLabelAlignment;
            GUILayout.FlexibleSpace();
            socketEndpointOptions.Reconnection = GUILayout.Toggle(socketEndpointOptions.Reconnection, "", this.guiSkin.customStyles[0]);
            GUILayout.EndHorizontal();
        }

        private void DrawResetStateToggle()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Reset State", "If 'true': resets the state of the flow to default"));
            GUILayout.FlexibleSpace();
            socketEndpointOptions.ResetState = GUILayout.Toggle(socketEndpointOptions.ResetState, "", this.guiSkin.customStyles[0]);
            GUILayout.EndHorizontal();
        }

        private void DrawResetContextToggle()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Reset Context", "If 'true': resets the context of the flow to default"));
            GUILayout.FlexibleSpace();
            socketEndpointOptions.ResetContext = GUILayout.Toggle(socketEndpointOptions.ResetContext, "", this.guiSkin.customStyles[0]);
            GUILayout.EndHorizontal();
        }

        private void DrawPassthroughIpField()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Passthrough IP");
            GUILayout.Label(new GUIContent("Optional", "Can be empty"), GUI.skin.GetStyle("optional"));
            GUILayout.EndHorizontal();
            socketEndpointOptions.PassthroughIP = GUILayout.TextField(socketEndpointOptions.PassthroughIP);
        }

        private void DrawAttachButton()
        {
            if (GUILayout.Button("Attach", guiSkin.GetStyle("AttachButton")))
            {
                AttachOptions();
            }
        }

        private void DrawCreateOptionsButton()
        {
            if (GUILayout.Button("Create Options", guiSkin.GetStyle("CreateButton")))
            {
                CreateOptions();
            }
        }

        private void DrawSelectedObjectLabel()
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Selected:");
            GUILayout.Label(selectedObject);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        #endregion

        private void AttachOptions()
        {
            if (EditorUtility.DisplayDialog("COGNIGY.AI", "Do you want to attach the Cognigy AI with these Settings to:\n" + Selection.activeTransform.gameObject.name, "Attach", "Cancel"))
            {
                string path = "Assets";
                string assetName;

                if (Selection.activeTransform != null)
                    assetName = Selection.activeTransform.gameObject.name;
                else
                {
                        assetName = "Endpoint_Options";
                }

                string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + "AI_" + assetName + ".asset");

                AssetDatabase.CreateAsset(socketEndpointOptions, assetPathAndName);

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                CognigyAI cognigyAI;

                if ((cognigyAI = Selection.activeTransform.gameObject.GetComponent<CognigyAI>()) == null)
                    cognigyAI = Selection.activeTransform.gameObject.AddComponent<CognigyAI>();

                cognigyAI.socketEndpointOptions = socketEndpointOptions;

                this.Close();
            }
        }

        private void CreateOptions()
        {
            if (EditorUtility.DisplayDialog("COGNIGY.AI", "Do you want to create these settings?", "Create", "Cancel"))
            {
                string path = "Assets";
                string assetName;

                if (Selection.activeTransform != null)
                    assetName = Selection.activeTransform.gameObject.name.Replace(" ", "_");
                else
                {
                        assetName = "Endpoint_Options";
                }

                string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + "AI_" + assetName + ".asset");

                AssetDatabase.CreateAsset(socketEndpointOptions, assetPathAndName);

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                this.Close();
            }
        }

    }
}
