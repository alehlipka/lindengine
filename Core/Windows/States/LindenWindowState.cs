using System;
using LindEngine.Core.Managers;
using OpenTK.Windowing.Common;

namespace LindEngine.Core.Windows.States;

public class LindenWindowState
{
    /// <summary>
    /// State name
    /// </summary>
    public readonly string Name;
    public readonly LindenWindow Window;
    private bool _isLoaded;

    protected LindenWindowState(string name, LindenWindow window)
    {
        Name = name;
        _isLoaded = false;
        Window = window;
        
        Console.WriteLine($"State created: {Name} ({window.Name} window)");
    }

    /// <summary>
    /// Run when window loaded
    /// </summary>
    public virtual void OnLoad()
    {
        _isLoaded = true;
        
        Gui.Manager.Resize(Window.ClientSize.X, Window.ClientSize.Y);
    }

    /// <summary>
    /// Run when window resized
    /// </summary>
    /// <param name="args">A <see cref="T:OpenTK.Windowing.Common.ResizeEventArgs" /> that contains the event data</param>
    public virtual void OnResize(ResizeEventArgs args)
    {
        if (!_isLoaded) return;
        
        Gui.Manager.Resize(Window.ClientSize.X, Window.ClientSize.Y);
    }

    /// <summary>
    /// Run when window frame updated
    /// </summary>
    /// <param name="args">The <see cref="T:OpenTK.Windowing.Common.FrameEventArgs" /> for this frame</param>
    public virtual void OnUpdate(FrameEventArgs args)
    {
        if (!_isLoaded) return;
    }
    
    /// <summary>
    /// Run when window frame rendered
    /// </summary>
    /// <param name="args">The <see cref="T:OpenTK.Windowing.Common.FrameEventArgs" /> for this frame</param>
    public virtual void OnRender(FrameEventArgs args)
    {
        if (!_isLoaded) return;
    }

    public virtual void OnMouseWheel(MouseWheelEventArgs e)
    {
        if (!_isLoaded) return;
        
        Gui.Manager.MouseScroll(e.Offset);
    }

    public virtual void OnTextInput(TextInputEventArgs e)
    {
        if (!_isLoaded) return;
        
        Gui.Manager.PressChar((char)e.Unicode);
    }
}