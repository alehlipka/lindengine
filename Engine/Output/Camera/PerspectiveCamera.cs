using OpenTK.Mathematics;

namespace Lindengine.Output.Camera;

public class PerspectiveCamera : Camera
{
    public PerspectiveCamera()
    {
        Position = Vector3.UnitZ;
        Target = Vector3.Zero;
        Up = Vector3.UnitY;
        NearClip = -100.0f;
        FarClip = 100.0f;
        Fov = MathHelper.PiOver4;
        
        ViewMatrix = Matrix4.LookAt(Position, Target, Up);
    }
    
    protected override void OnLoad() { }

    protected override void OnWindowResize(Vector2i size)
    {
        AspectRatio = size.X / (float)size.Y;
        ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(Fov), AspectRatio, NearClip, FarClip);
    }
    protected override void OnUpdate(double elapsedSeconds) { }
    protected override void OnRender(double elapsedSeconds) { }
    protected override void OnUnload() { }
}