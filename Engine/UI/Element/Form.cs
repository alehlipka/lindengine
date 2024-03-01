using Lindengine.Graphics;
using Lindengine.Graphics.Shader;
using OpenTK.Mathematics;

namespace Lindengine.UI.Element;

public class Form : UiElement
{
    public Form(Vector2i size, float border, ElementOrigin origin, Vector3 position, Texture texture, ShaderProgram shader)
        : base(size, border, origin, position, Vector3.Zero, Vector3.One, texture, shader) { }
}