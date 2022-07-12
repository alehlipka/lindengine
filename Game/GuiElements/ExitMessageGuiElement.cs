using ImGuiNET;
using LindEngine.Core;
using LindEngine.Core.GuiElements;
using LindEngine.Core.Windows.States;

namespace LindEngine.Game.GuiElements;

public class ExitMessageGuiElement: GuiElement
{
    public ExitMessageGuiElement(string name) : base(name)
    {
        Result.Add("IsOpen", false);
    }

    public override void Draw(LindenWindowState state)
    {
        int modalWidth = 300;
        int modalHeight = 92;
        int xCenter = (state.Window.ClientSize.X - modalWidth) / 2;
        int yCenter = (state.Window.ClientSize.Y - modalHeight) / 2;
        
        ImGui.OpenPopup("Exit");
        ImGui.SetNextWindowSize(new(modalWidth, modalHeight));
        ImGui.SetNextWindowPos(new(xCenter, yCenter));
        bool open = true;
        if (ImGui.BeginPopupModal("Exit", ref open, ImGuiWindowFlags.NoResize))
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

        Result["IsOpen"] = open;
    }
}