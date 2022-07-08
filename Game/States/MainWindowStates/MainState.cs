using LindEngine.Core.Windows.States;
using OpenTK.Windowing.Common;

namespace LindEngine.Game.States.MainWindowStates;

public class MainState : LindenWindowState
{
    public MainState(string name) : base(name)
    {
        
    }

    public override void OnLoad()
    {
        base.OnLoad();
    }

    public override void OnResize(ResizeEventArgs args)
    {
        base.OnResize(args);
    }

    public override void OnRender(FrameEventArgs args)
    {
        base.OnRender(args);
    }

    public override void OnUpdate(FrameEventArgs args)
    {
        base.OnUpdate(args);
    }
}