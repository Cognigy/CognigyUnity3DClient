using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AcapelaTTSOptions))]
public class AcapelaTTSOptionsEditor : OptionsInspector
{
    private AcapelaTTSOptions acapelaOptions;
    private AcapelaTTSOptionsDrawer optionsDrawer;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        optionsDrawer.DrawOptions(acapelaOptions);
    }

    public override void InitializeEditor()
    {
        acapelaOptions = (AcapelaTTSOptions)target;
        optionsDrawer = CreateInstance<AcapelaTTSOptionsDrawer>();
    }
}
