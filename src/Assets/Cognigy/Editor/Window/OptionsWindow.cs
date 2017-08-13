using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//Creates and/or attaches options
public abstract class OptionsWindow<TServiceComponent> : EditorWindow where TServiceComponent : ServiceComponent
{
    [SerializeField]
    protected GUISkin guiSkin;

    protected OptionsDrawer currentDrawer;
    protected ServiceOptions currentOptions;

    protected string selectedObject = "none";

    protected Dictionary<Type, ServiceOptions> typesAndServices = new Dictionary<Type, ServiceOptions>();

    protected bool hasSelection;

    public virtual void AttachOptions(ServiceOptions serviceOptions)
    {
        if (EditorUtility.DisplayDialog
            (
            serviceOptions.ServiceType.ToString().Replace("_", " "),
            serviceOptions.ServiceName + " Do you want to attach " + serviceOptions.ServiceType.ToString() + " to:\n" + Selection.activeTransform.gameObject.name,
            "Attach",
            "Cancel"
            ))
        {
            string typeAbbrev = string.Empty;

            switch (serviceOptions.ServiceType)
            {
                case ServiceType.Cognigy_AI:
                    typeAbbrev = "AI";
                    break;

                case ServiceType.Speech_To_Text:
                    typeAbbrev = "STT";
                    break;
                case ServiceType.Text_To_Speech:
                    typeAbbrev = "TTS";
                    break;
            }

            string path = "Assets";

            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + typeAbbrev + "_" + serviceOptions.ServiceName + ".asset");

            AssetDatabase.CreateAsset(serviceOptions, assetPathAndName);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            TServiceComponent serviceComponent;


            if ((serviceComponent = (TServiceComponent)Selection.activeTransform.gameObject.GetComponent(typeof(TServiceComponent))) == null)
                serviceComponent = (TServiceComponent)Selection.activeTransform.gameObject.AddComponent(typeof(TServiceComponent));

            serviceComponent.serviceOptions = serviceOptions;

            SetFocus(serviceComponent);

            this.Close();
        }


    }
    public virtual void CreateOptions(ServiceOptions serviceOptions)
    {
        if (EditorUtility.DisplayDialog
            (
            serviceOptions.ServiceType.ToString().Replace("_", " "),
            serviceOptions.ServiceName + " Do you want to create these " + serviceOptions.ServiceType.ToString().Replace("_", " ") + " settings?",
            "Create",
            "Cancel"
            ))
        {
            string typeAbbrev = string.Empty;

            switch (serviceOptions.ServiceType)
            {
                case ServiceType.Cognigy_AI:
                    typeAbbrev = "AI";
                    break;

                case ServiceType.Speech_To_Text:
                    typeAbbrev = "STT";
                    break;
                case ServiceType.Text_To_Speech:
                    typeAbbrev = "TTS";
                    break;
            }

            string path = "Assets";

            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + typeAbbrev + "_" + serviceOptions.ServiceName + ".asset");

            AssetDatabase.CreateAsset(serviceOptions, assetPathAndName);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            SetFocus(serviceOptions);
            EditorUtility.FocusProjectWindow();

            this.Close();
        }
    }

    public virtual void SetDrawer()
    {
        currentDrawer = CreateInstance<DefaultDrawer>();
        currentOptions = CreateInstance<ServiceOptions>();
    }

    public abstract void WindowSetup();

    public virtual void GuiSkinSetup() { }

    public virtual void DrawHeader() { }

    public virtual void DrawSelector() { }

    protected void Awake()
    {
        WindowSetup();
    }

    protected void OnGUI()
    {
        if (Selection.activeTransform != null)
        {
            hasSelection = true;
            selectedObject = Selection.activeTransform.gameObject.name;
        }
        else
        {
            hasSelection = false;
            selectedObject = "none";
        }

        GuiSkinSetup();
        DrawHeader();
        DrawSelector();

        OptionsLayout.HorizontalLine(this.guiSkin);

        GUILayout.Space(10);

        SetDrawer();
        currentDrawer.DrawOptions(currentOptions);
        Repaint();

        GUILayout.Space(12);

        OptionsLayout.HorizontalLine(this.guiSkin);

        GUILayout.Space(12);

        GUILayout.BeginHorizontal();
        DrawCreateButton();
        DrawAttachButton();
        GUILayout.EndHorizontal();
    }

    protected void OnInspectorUpdate()
    {
        Repaint();
    }

    protected void DrawCreateButton()
    {
        if (GUILayout.Button("Create Options", guiSkin.GetStyle("CreateButton")))
        {
            CreateOptions(currentOptions);
        }
    }

    protected void DrawAttachButton()
    {
        using (new EditorGUI.DisabledScope(hasSelection == false))
        {
            if (GUILayout.Button("Attach", guiSkin.GetStyle("AttachButton")))
            {
                AttachOptions(currentOptions);
            }
        }
    }

    protected void SetFocus(UnityEngine.Object target)
    {
        Selection.activeObject = target;
    }
}
