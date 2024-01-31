using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace lindengine.common.cameras
{
    internal class PerspectiveCamera(CameraType type, Vector3 position, Vector3 target, Vector3 up, float fov, float aspectRatio, float nearClip, float farClip)
        : Camera(type, position, target, up, fov, aspectRatio, nearClip, farClip)
    {
        protected override void OnContextResize(Camera camera, ResizeEventArgs args)
        {
            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(Fov), AspectRatio, NearClip, FarClip);
        }

        protected override void OnUpdateFrame(Camera camera, FrameEventArgs args)
        {
            //if (KeyboardState.IsKeyPressed(Keys.D1))
            //{
            //    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            //}
            //else if (KeyboardState.IsKeyPressed(Keys.D2))
            //{
            //    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            //}
            //else if (KeyboardState.IsKeyPressed(Keys.D3))
            //{
            //    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Point);
            //}

            //float cameraSpeed = (float)(50.0 * args.Time);

            //if (KeyboardState.IsKeyDown(Keys.W))
            //{
            //    Move(new Vector3(0, 0, -cameraSpeed));
            //}
            //if (KeyboardState.IsKeyDown(Keys.S))
            //{
            //    Move(new Vector3(0, 0, cameraSpeed));
            //}
            //if (KeyboardState.IsKeyDown(Keys.A))
            //{
            //    Move(new Vector3(-cameraSpeed, 0, 0));
            //}
            //if (KeyboardState.IsKeyDown(Keys.D))
            //{
            //    Move(new Vector3(cameraSpeed, 0, 0));
            //}

            ApplyTransforms();
        }

        private void Move(Vector3 offset)
        {
            Position += offset;
        }

        private void ApplyTransforms()
        {
            ViewMatrix = Matrix4.LookAt(Position, Target, Up);
            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(Fov), AspectRatio, NearClip, FarClip);
        }
    }
}
