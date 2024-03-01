using OpenTK.Mathematics;

namespace Lindengine.Output.Camera;

public class OrthographicCamera : Camera
{
    public OrthographicCamera()
    {
        Position = Vector3.UnitZ;
        Target = Vector3.Zero;
        Up = Vector3.UnitY;
        NearClip = -1.0f;
        FarClip = 1.0f;
        
        ViewMatrix = Matrix4.LookAt(Position, Target, Up);
    }
    
    protected override void OnLoad() { }

    protected override void OnWindowResize(Vector2i size)
    {
        AspectRatio = size.X / (float)size.Y;
        ProjectionMatrix = Matrix4.CreateOrthographicOffCenter(0, size.X, 0, size.Y, NearClip, FarClip);
    }
    protected override void OnUpdate(double elapsedSeconds) { }
    protected override void OnRender(double elapsedSeconds) { }
    protected override void OnUnload() { }
}