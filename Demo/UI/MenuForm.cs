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
        Border = new Vector4(120, 0, 0, 0);
        Texture = Lind.Engine.Resources.Load<Texture>(Path.Combine("Assets", "UI", "Panel", "form.jpg"));
        Font font = Lind.Engine.Resources.Load<Font>(Path.Combine("Assets", "Fonts", "OpenSansBold.ttf"));
        const int fontSize = 22;
        TextBuilder textBuilder = new();

        Vector2i textSize = new(Size.X - 10, fontSize);
        UiElement title = new(textSize, shader)
        {
            Position = new Vector3(Size.X / 2.0f, Size.Y - 23, 0),
            Origin = ElementOrigin.Center,
            Texture = new Texture(textBuilder.Draw("MAIN MENU", font, fontSize, Color4.DarkGray, textSize, TextAlign.Center))
        };

        UiElement button = new(new Vector2i(208, 30), shader)
        {
            Position = new Vector3(Size.X / 2.0f, 23, 0),
            Origin = ElementOrigin.Center,
            Texture = Lind.Engine.Resources.Load<Texture>(Path.Combine("Assets", "UI", "Panel", "button.png"))
        };
        
        textSize = new Vector2i(205, 22);
        UiElement buttonTitle = new(textSize, shader)
        {
            Position = new Vector3(104.0f, 15.0f, 0),
            Origin = ElementOrigin.Center,
            Texture = new Texture(textBuilder.Draw("Exit", font, 22, Color4.DarkGray, textSize, TextAlign.Center))
        };
        button.AddChildren(buttonTitle);
        
        AddChildren(title);
        AddChildren(button);
    }

    protected override void OnWindowResize(Vector2i size)
    {
        Position = new Vector3(size) / 2;
    }
}