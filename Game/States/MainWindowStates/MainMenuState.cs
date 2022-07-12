using LindEngine.Core.Managers;
using LindEngine.Core.Windows;
using LindEngine.Core.Windows.States;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace LindEngine.Game.States.MainWindowStates;

public class MainMenuState : LindenWindowState
{
    private bool _exitMessageTrigger;
    
    public MainMenuState(string name, LindenWindow window) : base(name, window)
    {
    }

    public override void OnRender(FrameEventArgs args)
    {
        base.OnRender(args);

        if (_exitMessageTrigger)
        {
            Gui.Manager.Select("ExitMessage");
            Gui.Manager.SelectedElement.Draw(this);
            _exitMessageTrigger = Gui.Manager.SelectedElement.Result["IsOpen"];
        }

        Gui.Manager.Render();
    }

    public override void OnUpdate(FrameEventArgs args)
    {
        base.OnUpdate(args);

        if (Window.IsKeyPressed(Keys.Escape))
        {
            _exitMessageTrigger = true;
        }

        Gui.Manager.Update(Window, (float)args.Time);
    }
}