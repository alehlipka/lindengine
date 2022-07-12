using ImGuiNET;
using LindEngine.Core;
using LindEngine.Core.Managers;
using LindEngine.Core.Windows;
using LindEngine.Core.Windows.States;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace LindEngine.Game.States.MainWindowStates;

public class MainMenuState : LindenWindowState
{
    private bool _isExitTriggered = false;

    public MainMenuState(string name, LindenWindow window) : base(name, window)
    {
    }

    private void DrawExitPopup()
    {
        ImGui.OpenPopup("Exit");
        ImGui.SetNextWindowSize(new(300, 92));
        ImGui.SetNextWindowPos(new((Window.ClientSize.X - 300) / 2, (Window.ClientSize.Y - 92) / 2));
        bool open = true;
        if (ImGui.BeginPopupModal("Exit", ref open))
        {
            ImGui.Text("Did you realy want to exit?");
            if (ImGui.Button("Yes", new(176, 40))) {
                IsLoaded = false;
                Application.Starter.Exit();
            }
            ImGui.SameLine();
            if (ImGui.Button("No", new(100, 40))) {
                open = false;
            }
            ImGui.EndPopup();
        }

        _isExitTriggered = open;
    }

    public override void OnRender(FrameEventArgs args)
    {
        base.OnRender(args);

        if (_isExitTriggered) DrawExitPopup();

        Gui.Manager.Render();
    }

    public override void OnUpdate(FrameEventArgs args)
    {
        base.OnUpdate(args);

        Gui.Manager.Update(Window, (float)args.Time);

        if (Window.IsKeyPressed(Keys.Escape)) _isExitTriggered = true;
    }
}