using Lindengine.Graphics;
using Lindengine.Graphics.Shader;
using Lindengine.UI;
using OpenTK.Mathematics;

namespace Demo.UI;

public class Background : UiElement
{
    public Background(Vector2i size, float border, Texture texture, ShaderProgram shader) : base(size, border, texture, shader)
    {
        
    }
}