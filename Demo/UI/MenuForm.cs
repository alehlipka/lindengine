using Lindengine.Core;
using Lindengine.Graphics;
using Lindengine.Graphics.Font;
using Lindengine.Graphics.Shader;
using Lindengine.UI;
using OpenTK.Mathematics;

namespace Demo.UI;

public class MenuForm : UiElement
{
    public MenuForm(Vector2i size, ShaderProgram shader) : base(size, shader)
    {
        Origin = ElementOrigin.Center;
        Position = new Vector3(Size.X, Size.Y, 0) / 2;
        Border = 16;
        Texture = Lind.Engine.Resources.Load<Texture>(Path.Combine("Assets", "UI", "Panel", "form.jpg"));
        Font font = Lind.Engine.Resources.Load<Font>(Path.Combine("Assets", "Fonts", "OpenSansBold.ttf"));
        const int fontSize = 32;
        TextBuilder textBuilder = new();

        Vector2i textSize = new(Size.X - 10, fontSize);
        UiElement title = new(textSize, shader)
        {
            Position = new Vector3(Size.X / 2.0f, Size.Y - 25, 0),
            Origin = ElementOrigin.Center,
            Texture = new Texture(textBuilder.Draw("MAIN MENU", font, 32, Color4.DarkGoldenrod, textSize, TextAlign.Center))
        };
        
        AddChildren(title);
    }

    protected override void OnWindowResize(Vector2i size)
    {
        Position = new Vector3(size) / 2;
    }
}