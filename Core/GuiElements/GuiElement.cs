using System.Collections.Generic;
using LindEngine.Core.Windows.States;

namespace LindEngine.Core.GuiElements;

public class GuiElement
{
    public readonly string Name;

    public Dictionary<string, bool> Result { get; }

    protected GuiElement(string name)
    {
        Name = name;
        Result = new Dictionary<string, bool>();
    }

    public virtual void Draw(LindenWindowState state)
    {
        
    }
}