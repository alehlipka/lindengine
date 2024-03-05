using Lindengine.Core;
using Lindengine.Graphics;
using Lindengine.Graphics.Shader;
using Lindengine.Input;
using Lindengine.Output.Camera;
using Lindengine.UI;
using OpenTK.Mathematics;

namespace Demo.UI;

public class Pointer : UiElement
{
    public Pointer(Vector2i size, ShaderProgram shader) : base(size, shader)
    {
        Scale = new Vector3(0.6f, 0.6f, 1);
        Origin = ElementOrigin.TopLeft;
        Vector2i mousePosition = InputManager.MousePositionBottomLeft();
        Position = new Vector3(mousePosition);
        Texture = Lind.Engine.Resources.Load<Texture>(Path.Combine("Assets", "UI", "Pointer", "default.png"));
    }
    
    protected override void OnRender(Camera camera, double elapsedSeconds)
    {
        Vector2i mousePosition = InputManager.MousePositionBottomLeft();
        Position = new Vector3(mousePosition);
    }
}