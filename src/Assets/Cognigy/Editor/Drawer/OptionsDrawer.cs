using UnityEditor;

/// <summary>
/// Draws the specific options into the editor (Window/Inspector)
/// </summary>
public abstract class OptionsDrawer : EditorWindow
{
    public abstract void DrawOptions<TOptions>(TOptions serviceOptions) where TOptions : ServiceOptions;
}
