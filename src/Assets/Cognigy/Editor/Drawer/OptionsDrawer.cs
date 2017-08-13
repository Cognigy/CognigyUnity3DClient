using System;
using UnityEditor;

public class OptionsDrawer : EditorWindow
{
    protected OptionsWindow optionsWindow;

    public void SetWindow(OptionsWindow optionsWindow)
    {
        this.optionsWindow = optionsWindow;
    }

    public virtual void Initialize() { }

    public virtual void DrawOptions() { }

    public virtual ServiceOptions GetOptions()
    {
        throw new NotImplementedException();
    }
}
