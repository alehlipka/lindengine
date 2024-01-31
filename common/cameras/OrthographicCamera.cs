using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace lindengine.common.cameras
{
    public class OrthographicCamera(string name, Vector3 position, Vector3 target, Vector3 up, float fov, float aspectRatio, float nearClip, float farClip)
        : Camera(name, position, target, up, fov, aspectRatio, nearClip, farClip)
    {
        protected override void OnContextResize(Camera camera, ResizeEventArgs args)
        {
            ProjectionMatrix = Matrix4.CreateOrthographicOffCenter(0, args.Width, 0, args.Height, NearClip, FarClip);
        }
    }
}
