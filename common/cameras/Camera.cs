using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace lindengine.common.cameras
{
    internal class Camera
    {
        public CameraType Type;
        public Matrix4 ViewMatrix = Matrix4.Identity;
        public Matrix4 ProjectionMatrix = Matrix4.Identity;

        protected Vector3 Position;
        protected Vector3 Target;
        protected Vector3 Up;
        protected float Fov;
        protected float AspectRatio;
        protected float NearClip;
        protected float FarClip;

        private delegate void CameraDelegate(Camera camera);
        private delegate void CameraContextResizeDelegate(Camera camera, ResizeEventArgs args);
        private delegate void CameraFrameDelegate(Camera camera, FrameEventArgs args);

        private event CameraDelegate? LoadEvent;
        private event CameraDelegate? UnloadEvent;
        private event CameraContextResizeDelegate? ContextResizeEvent;
        private event CameraFrameDelegate? UpdateEvent;
        private event CameraFrameDelegate? RendereEvent;

        private bool _isLoaded;

        public Camera(CameraType type, Vector3 position, Vector3 target, Vector3 up, float fov, float aspectRatio, float nearClip, float farClip)
        {
            Type = type;
            Position = position;
            Target = target;
            Up = up;
            Fov = fov;
            AspectRatio = aspectRatio;
            NearClip = nearClip;
            FarClip = farClip;

            ViewMatrix = Matrix4.LookAt(Position, Target, Up);
        }

        public void Load()
        {
            if (!_isLoaded)
            {
                LoadEvent += OnLoad;
                ContextResizeEvent += OnContextResize;
                UpdateEvent += OnUpdateFrame;
                RendereEvent += OnRenderFrame;
                UnloadEvent += OnUnload;

                LoadEvent?.Invoke(this);
                _isLoaded = true;
            }
        }

        public void Resize(ResizeEventArgs args)
        {
            if (_isLoaded)
            {
                AspectRatio = args.Width / (float)args.Height;
                ContextResizeEvent?.Invoke(this, args);
            }
        }

        public void Update(FrameEventArgs args)
        {
            if (_isLoaded)
            {
                UpdateEvent?.Invoke(this, args);
            }
        }

        public void Render(FrameEventArgs args)
        {
            if (_isLoaded)
            {
                RendereEvent?.Invoke(this, args);
            }
        }

        public void Unload()
        {
            if (_isLoaded)
            {
                UnloadEvent?.Invoke(this);

                LoadEvent -= OnLoad;
                ContextResizeEvent -= OnContextResize;
                UpdateEvent -= OnUpdateFrame;
                RendereEvent -= OnRenderFrame;
                UnloadEvent -= OnUnload;

                _isLoaded = false;
            }
        }

        protected virtual void OnLoad(Camera camera) { }
        protected virtual void OnContextResize(Camera camera, ResizeEventArgs args) { }
        protected virtual void OnUpdateFrame(Camera camera, FrameEventArgs args) { }
        protected virtual void OnRenderFrame(Camera camera, FrameEventArgs args) { }
        protected virtual void OnUnload(Camera camera) { }
    }
}
