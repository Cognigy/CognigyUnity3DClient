using UnityEditor;
using UnityEngine;

//Creates and/or attaches options
public class OptionsWindow : EditorWindow
{
    protected OptionsDrawer currentDrawer;
    protected string serviceType = "";
    public bool hasSelection;

    public virtual void AttachOptions(ServiceOptions serviceOptions) { }
    public virtual void CreateOptions(ServiceOptions serviceOptions)
    {
        string path = "Assets";

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + serviceType + "_" + serviceOptions.ServiceName + ".asset");

        AssetDatabase.CreateAsset(serviceOptions, assetPathAndName);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Selection.activeObject = serviceOptions;
        EditorUtility.FocusProjectWindow();
    }

    public virtual OptionsDrawer SetDrawer()
    {
        return null;
    }

    public virtual void DrawHeader() { }

    public virtual void DrawSelector() { }

    protected void DrawCreateButton()
    {
        if (GUILayout.Button("Create Options"))
        {
            CreateOptions(currentDrawer.GetOptions());

            this.Close();
        }
    }

    protected void DrawAttachButton()
    {
        using (new EditorGUI.DisabledScope(hasSelection == false))
        {
            if (GUILayout.Button("Attach " + serviceType))
            {
                AttachOptions(currentDrawer.GetOptions());

                this.Close();
            }
        }
    }

    protected void OnGUI()
    {
        if (Selection.activeTransform != null)
            hasSelection = true;
        else
            hasSelection = false;

        DrawHeader();
        DrawSelector();

        currentDrawer = SetDrawer();
        currentDrawer.DrawOptions();

        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        DrawCreateButton();
        DrawAttachButton();
        GUILayout.EndHorizontal();
    }

    protected void OnInspectorUpdate()
    {
        Repaint();
    }
}
