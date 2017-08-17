using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class OptionsLayout
{
    public static void HorizontalLine(GUISkin guiSkin)
    {
        GUILayout.Box("", guiSkin.GetStyle("seperator"), new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(2) });
    }
}
