using ImGuiNET;
using LindEngine.Core;
using LindEngine.Core.GuiElements;
using LindEngine.Core.Windows.States;

namespace LindEngine.Game.GuiElements;

public class ExitMessageGuiElement: GuiElement
{
    public ExitMessageGuiElement(string name) : base(name)
    {
    }

    public override void Draw(LindenWindowState state)
    {
        ImGui.OpenPopup("Exit");
        ImGui.SetNextWindowSize(new(300, 92));
        ImGui.SetNextWindowPos(new((state.Window.ClientSize.X - 300) / 2, (state.Window.ClientSize.Y - 92) / 2));
        bool open = true;
        if (ImGui.BeginPopupModal("Exit", ref open))
        {
            ImGui.Text("Did you realy want to exit?");
            if (ImGui.Button("Yes", new(176, 40))) {
                Application.Starter.Exit();
            }
            ImGui.SameLine();
            if (ImGui.Button("No", new(100, 40))) {
                open = false;
            }
            ImGui.EndPopup();
        }
    }
}