using System.Numerics;
using ImGuiNET;
using LindEngine.Core;
using LindEngine.Core.GuiElements;
using LindEngine.Core.Windows;
using LindEngine.Core.Windows.States;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;

namespace LindEngine.Game.GuiElements;

public class MainMenuGuiElement: GuiElement
{
    private int _windowHeight;
    private int _windowWidth;
    private bool _exitTriggered;
    
    public MainMenuGuiElement(string name) : base(name) { }

    private void SetStyles()
    {
        ImGuiStylePtr style = ImGui.GetStyle();
        style.Colors[(int)ImGuiCol.Button] = new Vector4(0.16f, 0.16f, 0.16f, 1.0f);
        style.Colors[(int)ImGuiCol.ButtonHovered] = new Vector4(0.3f, 0.3f, 0.3f, 1.0f);
        style.Colors[(int)ImGuiCol.ButtonActive] = new Vector4(0.2f, 0.2f, 0.2f, 1.0f);
    }

    public override void Draw(LindenWindowState state)
    {
        _windowHeight = state.Window.ClientSize.Y;
        _windowWidth = state.Window.ClientSize.X;
        
        SetStyles();
        
        ImGui.Begin("Main menu",
            ImGuiWindowFlags.None
            | ImGuiWindowFlags.NoCollapse
            | ImGuiWindowFlags.NoMove
            | ImGuiWindowFlags.NoResize
            | ImGuiWindowFlags.NoTitleBar
            | ImGuiWindowFlags.NoBackground
        );
        ImGui.SetWindowPos(new(0, 0));
        ImGui.SetWindowSize(new(_windowWidth, _windowHeight));
        DebugInfo(state);
        ExitButton();
        ExitModal();
        ImGui.End();
    }

    private void DebugInfo(LindenWindowState state)
    {
        ImGui.Text("FPS:");
        ImGui.SameLine();
        ImGui.PushStyleColor(ImGuiCol.Text, new Vector4(0, 1, 0, 1));
        ImGui.Text($"{ FpsCounter.FPS } [{ FpsCounter.Max }:{ FpsCounter.Min }]");
        ImGui.PopStyleColor();
        
        ImGui.Text("OpenGL version:");
        ImGui.SameLine();
        ImGui.PushStyleColor(ImGuiCol.Text, new Vector4(0, 1, 0, 1));
        ImGui.Text(GL.GetString(StringName.Version));
        ImGui.PopStyleColor();
        
        ImGui.Text("VSync:");
        ImGui.SameLine();
        ImGui.PushStyleColor(ImGuiCol.Text, new Vector4(0, 1, 0, 1));
        ImGui.Text(state.Window.VSync == VSyncMode.Off ? "disabled" : "enabled");
        ImGui.PopStyleColor();
        
        ImGui.Text("Window name:");
        ImGui.SameLine();
        ImGui.PushStyleColor(ImGuiCol.Text, new Vector4(0, 1, 0, 1));
        ImGui.Text(state.Window.Name);
        ImGui.PopStyleColor();
        
        ImGui.Text("State name:");
        ImGui.SameLine();
        ImGui.PushStyleColor(ImGuiCol.Text, new Vector4(0, 1, 0, 1));
        ImGui.Text(state.Name);
        ImGui.PopStyleColor();
        
        ImGui.Text("Fullscreen toggle:");
        ImGui.SameLine();
        ImGui.PushStyleColor(ImGuiCol.Text, new Vector4(0, 1, 0, 1));
        ImGui.Text("Tab");
        ImGui.PopStyleColor();
    }

    private void ExitButton()
    {
        float cursorPosY = ImGui.GetCursorPosY();
        ImGui.SetCursorPosY(_windowHeight - 60);
        if (ImGui.Button("Выйти из игры", new Vector2(300, 50))) _exitTriggered = true;
        ImGui.SetCursorPosY(cursorPosY);
    }

    private void ExitModal()
    {
        if (!_exitTriggered) return;
        
        int modalWidth = 244;
        int modalHeight = 74;
        int buttonWidth = 110;
        int buttonHeight = 30;
            
        ImGui.OpenPopup("Exit popup");
        ImGui.SetNextWindowSize(new Vector2(modalWidth, modalHeight));
        ImGui.SetNextWindowPos(new Vector2((_windowWidth - modalWidth) / 2.0f, (_windowHeight - modalHeight) / 2.0f));
        if (!ImGui.BeginPopupModal("Exit popup", ref _exitTriggered,
                ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoTitleBar)) return;
        
        ImGui.Text("Вы действительно хотите выйти?");
        ImGui.Separator();
        ImGui.Spacing();
        if (ImGui.Button("Да", new Vector2(buttonWidth, buttonHeight))) Application.Starter.Exit();
        ImGui.SameLine();
        if (ImGui.Button("Нет", new Vector2(buttonWidth, buttonHeight))) _exitTriggered = false;
        ImGui.EndPopup();
    }
}