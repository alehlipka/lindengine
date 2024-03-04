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
        Texture texture1 = Lind.Engine.Resources.Load<Texture>(Path.Combine("Assets", "UI", "Panels", "panel_7.png"));
        UiElement insideForm1 = new(new Vector2i(350, 275), border, texture1, shader)
        {
            Position = new Vector3(25, 100, 0)
        };
        Texture texture2 = Lind.Engine.Resources.Load<Texture>(Path.Combine("Assets", "UI", "Panels", "panel_12.png"));
        UiElement insideForm2 = new(new Vector2i(350, 60), border, texture2, shader)
        {
            Position = new Vector3(25, 25, 0)
        };
        Texture texture3 = Lind.Engine.Resources.Load<Texture>(Path.Combine("Assets", "UI", "Panels", "panel_0.png"));
        UiElement insideInsideForm = new(new Vector2i(100, 100), border, texture3, shader)
        {
            Origin = ElementOrigin.Center,
            Position = new Vector3(350, 275, 0) / 2
        };
        insideForm1.AddChildren(insideInsideForm);
        
        AddChildren(insideForm1);
        AddChildren(insideForm2);
    }
}