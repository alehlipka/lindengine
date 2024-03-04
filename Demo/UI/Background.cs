using Lindengine.Graphics.Shader;
using Lindengine.UI;
using OpenTK.Mathematics;

namespace Demo.UI;

public class Background(Vector2i size, ShaderProgram shader) : UiElement(size, shader)
{
    protected override void OnWindowResize(Vector2i size)
    {
        Size = size;
    }
}