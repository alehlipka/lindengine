using System;
using Dear_ImGui_Sample;
using ImGuiNET;
using LindEngine.Core;
using LindEngine.Core.Windows;
using LindEngine.Core.Windows.States;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace LindEngine.Game.States.MainWindowStates;

public class MainState : LindenWindowState
{
    ImGuiController _controller;
    
    public MainState(string name, LindenWindow window) : base(name, window)
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
        Console.WriteLine(Window.ClientSize.X + " " + Window.ClientSize.Y);

        _controller.WindowResized(Window.ClientSize.X, Window.ClientSize.Y);
    }

    public override void OnRender(FrameEventArgs args)
    {
        base.OnRender(args);

        ImGui.ShowDemoWindow();

        _controller.Render();

        ImGuiController.CheckGLError("End of frame");
    }

    public override void OnUpdate(FrameEventArgs args)
    {
        base.OnUpdate(args);
        
        _controller.Update(Window, (float)args.Time);

        if (!Window.IsKeyPressed(Keys.Escape)) return;
        IsLoaded = false;
        Application.Starter.Exit();
    }
}