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

        ImGui.Begin("LindEngine Left",
            ImGuiWindowFlags.Modal |
            ImGuiWindowFlags.NoCollapse |
            ImGuiWindowFlags.NoMove |
            ImGuiWindowFlags.NoResize
        );
        ImGui.SetWindowSize(new System.Numerics.Vector2(200, Window.ClientSize.Y));
        ImGui.SetWindowPos(new System.Numerics.Vector2(0, 0));
        ImGui.Text("Hello!");
        ImGui.End();

        ImGui.Begin("LindEngine Right",
            ImGuiWindowFlags.Modal |
            ImGuiWindowFlags.NoCollapse |
            ImGuiWindowFlags.NoMove |
            ImGuiWindowFlags.NoResize
        );
        ImGui.SetWindowSize(new System.Numerics.Vector2(200, Window.ClientSize.Y));
        ImGui.SetWindowPos(new System.Numerics.Vector2(Window.ClientSize.X - 200, 0));
        ImGui.Text("Hello!");
        ImGui.End();

        ImGui.Begin("LindEngine Top",
            ImGuiWindowFlags.Modal |
            ImGuiWindowFlags.NoCollapse |
            ImGuiWindowFlags.NoMove |
            ImGuiWindowFlags.NoResize
        );
        ImGui.SetWindowSize(new System.Numerics.Vector2(Window.ClientSize.X - 400, 50));
        ImGui.SetWindowPos(new System.Numerics.Vector2(200, 0));
        ImGui.Text("Hello!");
        ImGui.End();

        ImGui.Begin("LindEngine Bottom",
            ImGuiWindowFlags.Modal |
            ImGuiWindowFlags.NoCollapse |
            ImGuiWindowFlags.NoMove |
            ImGuiWindowFlags.NoResize
        );
        ImGui.SetWindowSize(new System.Numerics.Vector2(Window.ClientSize.X - 400, 50));
        ImGui.SetWindowPos(new System.Numerics.Vector2(200, Window.ClientSize.Y - 50));
        ImGui.Text("Hello!");
        ImGui.End();

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