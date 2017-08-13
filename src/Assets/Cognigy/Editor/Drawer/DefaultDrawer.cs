using UnityEngine;

public class DefaultDrawer : OptionsDrawer
{
    public override void DrawOptions<TOptions>(TOptions serviceOptions)
    {
        DrawDefaultLabel();
    }

    private void DrawDefaultLabel()
    {
        GUILayout.Label("Nothing here.");
    }
}
