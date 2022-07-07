using OpenTK.Windowing.Common;

namespace LindEngine.Core.Windows.States;

public class LindenWindowState
{
    /// <summary>
    /// State name
    /// </summary>
    public readonly string Name;

    public LindenWindowState(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Run when window loaded
    /// </summary>
    public virtual void OnLoad()
    {
        
    }

    /// <summary>
    /// Run when window resized
    /// </summary>
    /// <param name="args">A <see cref="T:OpenTK.Windowing.Common.ResizeEventArgs" /> that contains the event data</param>
    public virtual void OnResize(ResizeEventArgs args)
    {
        
    }

    /// <summary>
    /// Run when window frame updated
    /// </summary>
    /// <param name="args">The <see cref="T:OpenTK.Windowing.Common.FrameEventArgs" /> for this frame</param>
    public virtual void OnUpdate(FrameEventArgs args)
    {
        
    }
    
    /// <summary>
    /// Run when window frame rendered
    /// </summary>
    /// <param name="args">The <see cref="T:OpenTK.Windowing.Common.FrameEventArgs" /> for this frame</param>
    public virtual void OnRender(FrameEventArgs args)
    {
        
    }
}