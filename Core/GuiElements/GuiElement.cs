using LindEngine.Core.Windows.States;

namespace LindEngine.Core.GuiElements;

public class GuiElement
{
    public readonly string Name;

    public GuiElement(string name)
    {
        Name = name;
    }

    public virtual void Draw(LindenWindowState state)
    {
        
    }
}