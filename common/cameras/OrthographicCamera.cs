using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace lindengine.common.cameras
{
    internal class OrthographicCamera(CameraType type, Vector3 position, Vector3 target, Vector3 up, float fov, float aspectRatio, float nearClip, float farClip)
        : Camera(type, position, target, up, fov, aspectRatio, nearClip, farClip)
    {
        protected override void OnContextResize(Camera camera, ResizeEventArgs args)
        {
            ProjectionMatrix = Matrix4.CreateOrthographicOffCenter(0, args.Width, 0, args.Height, NearClip, FarClip);
        }
    }
}
