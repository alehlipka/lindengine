using LindEngine.Core.Managers;
using LindEngine.Core.Windows;
using LindEngine.Core.Windows.States;
using OpenTK.Windowing.Common;

namespace LindEngine.Game.States.MainWindowStates;

public class MainMenuState : LindenWindowState
{
    public MainMenuState(string name, LindenWindow window) : base(name, window)
    {
    }

    public override void OnRender(FrameEventArgs args)
    {
        base.OnRender(args);

        Gui.Manager.Select("ExitMessage");
        Gui.Manager.SelectedElement.Draw(this);

        Gui.Manager.Render();
    }

    public override void OnUpdate(FrameEventArgs args)
    {
        base.OnUpdate(args);

        Gui.Manager.Update(Window, (float)args.Time);
    }
}