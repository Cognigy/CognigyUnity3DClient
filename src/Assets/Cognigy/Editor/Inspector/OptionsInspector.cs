using UnityEditor;
using UnityEngine;

public abstract class OptionsInspector : Editor
{
    [SerializeField]
    protected GUISkin guiSkin;

    public abstract void InitializeEditor();

    public override void OnInspectorGUI()
    {
        GUI.skin = guiSkin;
    }

    protected void Awake()
    {
        InitializeEditor();
    }
}
