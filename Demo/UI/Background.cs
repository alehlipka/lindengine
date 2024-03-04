using Lindengine.Core;
using Lindengine.Graphics;
using Lindengine.Graphics.Shader;
using Lindengine.UI;
using OpenTK.Mathematics;

namespace Demo.UI;

public class Background : UiElement
{
    public Background(Vector2i size, ShaderProgram shader) : base(size, shader)
    {
        Texture = Lind.Engine.Resources.Load<Texture>(Path.Combine("Assets", "UI", "Background", "default.png"));
    }

    protected override void OnWindowResize(Vector2i size)
    {
        Size = size;
    }
}