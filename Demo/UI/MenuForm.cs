using Lindengine.Core;
using Lindengine.Graphics;
using Lindengine.Graphics.Shader;
using Lindengine.UI;
using OpenTK.Mathematics;

namespace Demo.UI;

public class MenuForm : UiElement
{
    public MenuForm(Vector2i size, ShaderProgram shader) : base(size, shader)
    {
        UiElement insideForm1 = new(new Vector2i(350, 275), shader)
        {
            Position = new Vector3(25, 100, 0),
            Border = 16,
            Texture = Lind.Engine.Resources.Load<Texture>(Path.Combine("Assets", "UI", "Panels", "panel_7.png"))
        };
        UiElement insideForm2 = new(new Vector2i(350, 60), shader)
        {
            Position = new Vector3(25, 25, 0),
            Border = 16,
            Texture = Lind.Engine.Resources.Load<Texture>(Path.Combine("Assets", "UI", "Panels", "panel_12.png"))
        };
        UiElement insideInsideForm = new(new Vector2i(100, 100), shader)
        {
            Origin = ElementOrigin.Center,
            Position = new Vector3(350, 275, 0) / 2,
            Border = 16,
            Texture = Lind.Engine.Resources.Load<Texture>(Path.Combine("Assets", "UI", "Panels", "panel_0.png"))
        };
        insideForm1.AddChildren(insideInsideForm);
        
        AddChildren(insideForm1);
        AddChildren(insideForm2);
    }
}