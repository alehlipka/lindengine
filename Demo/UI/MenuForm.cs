using Lindengine.Core;
using Lindengine.Graphics;
using Lindengine.Graphics.Shader;
using Lindengine.UI;
using OpenTK.Mathematics;

namespace Demo.UI;

public class MenuForm : UiElement
{
    public MenuForm(Vector2i size, float border, Texture texture, ShaderProgram shader) : base(size, border, texture, shader)
    {
        Texture panel1 = Lind.Engine.Resources.Load<Texture>(Path.Combine("Assets", "UI", "Panels", "panel_1.png"));
        Texture panel2 = Lind.Engine.Resources.Load<Texture>(Path.Combine("Assets", "UI", "Panels", "panel_2.png"));
        Texture panel3 = Lind.Engine.Resources.Load<Texture>(Path.Combine("Assets", "UI", "Panels", "panel_3.png"));
        
        UiElement insideForm1 = new(new Vector2i(350, 275), 32, panel1, shader)
        {
            Position = new Vector3(25, 100, 0)
        };
        UiElement insideForm2 = new(new Vector2i(350, 60), 12, panel2, shader)
        {
            Position = new Vector3(25, 25, 0)
        };
        UiElement insideInsideForm = new(new Vector2i(100, 100), 16, panel3, shader)
        {
            Origin = ElementOrigin.Center,
            Position = new Vector3(350, 275, 0) / 2
        };
        insideForm1.AddChildren(insideInsideForm);
        
        AddChildren(insideForm1);
        AddChildren(insideForm2);
    }
}