using Dear_ImGui_Sample;
using ImGuiNET;
using LindEngine.Core;
using LindEngine.Core.Windows;
using LindEngine.Core.Windows.States;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace LindEngine.Game.States.MainWindowStates;

public class MainMenuState : LindenWindowState
{
    private ImGuiController _controller;
    private bool _isNeetExit = false;

    public MainMenuState(string name, LindenWindow window) : base(name, window)
    {
        Window.MouseWheel += WindowOnMouseWheel;
        Window.TextInput += WindowOnTextInput;
    }

    private void WindowOnTextInput(TextInputEventArgs obj)
    {
        _controller.PressChar((char)obj.Unicode);
    }

    private void WindowOnMouseWheel(MouseWheelEventArgs obj)
    {
        _controller.MouseScroll(obj.Offset);
    }

    public override void OnLoad()
    {
        base.OnLoad();

        _controller = new ImGuiController(Window.ClientSize.X, Window.ClientSize.Y);
    }

    public override void OnResize(ResizeEventArgs args)
    {
        base.OnResize(args);

        _controller.WindowResized(Window.ClientSize.X, Window.ClientSize.Y);
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

        _isNeetExit = open;
    }

    public override void OnRender(FrameEventArgs args)
    {
        base.OnRender(args);

        if (_isNeetExit) DrawExitPopup();

        _controller.Render();

        ImGuiController.CheckGLError("End of frame");
    }

    public override void OnUpdate(FrameEventArgs args)
    {
        base.OnUpdate(args);

        _controller.Update(Window, (float)args.Time);

        if (Window.IsKeyPressed(Keys.Escape)) _isNeetExit = true;
    }
}