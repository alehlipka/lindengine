using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace lindengine.core.camera
{
    internal class Camera
    {
        public string Name;
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

        private event CameraDelegate? CreateEvent;
        private event CameraDelegate? LoadEvent;
        private event CameraDelegate? UnloadEvent;
        private event CameraContextResizeDelegate? ContextResizeEvent;
        private event CameraFrameDelegate? UpdateEvent;
        private event CameraFrameDelegate? RendereEvent;

        private bool _isLoaded;

        private const string consoleStarter = "├── ";

        public Camera(string name, Vector3 position, Vector3 target, Vector3 up, float fov, float aspectRatio, float nearClip, float farClip)
        {
            Name = name.ToLower();
            Position = position;
            Target = target;
            Up = up;
            Fov = fov;
            AspectRatio = aspectRatio;
            NearClip = nearClip;
            FarClip = farClip;

            ViewMatrix = Matrix4.LookAt(Position, Target, Up);

            CreateEvent += OnCreate;
            CreateEvent?.Invoke(this);
            Console.WriteLine(consoleStarter + $"Camera created: {Name}");
        }

        public void Load()
        {
            if (!_isLoaded)
            {
                Console.WriteLine(consoleStarter + $"Camera loading: {Name}");
                LoadEvent += OnLoad;
                ContextResizeEvent += OnContextResize;
                UpdateEvent += OnUpdateFrame;
                RendereEvent += OnRenderFrame;
                UnloadEvent += OnUnload;

                LoadEvent?.Invoke(this);
                _isLoaded = true;
                Console.WriteLine(consoleStarter + $"Camera loaded: {Name}");
            }
        }

        public void Resize(ResizeEventArgs e)
        {
            if (_isLoaded)
            {
                Console.WriteLine(consoleStarter + $"Camera resizing: {Name} {e.Width}x{e.Height}");
                
                AspectRatio = e.Width / (float)e.Height;
                ContextResizeEvent?.Invoke(this, e);
                
                Console.WriteLine(consoleStarter + $"Camera resized: {Name} {e.Width}x{e.Height}");
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
                Console.WriteLine(consoleStarter + $"Camera unloading: {Name}");
                UnloadEvent?.Invoke(this);

                CreateEvent -= OnCreate;
                LoadEvent -= OnLoad;
                ContextResizeEvent -= OnContextResize;
                UpdateEvent -= OnUpdateFrame;
                RendereEvent -= OnRenderFrame;
                UnloadEvent -= OnUnload;

                _isLoaded = false;
                Console.WriteLine(consoleStarter + $"Camera unloaded: {Name}");
            }
        }

        protected virtual void OnCreate(Camera camera) { }
        protected virtual void OnLoad(Camera camera) { }
        protected virtual void OnContextResize(Camera camera, ResizeEventArgs args) { }
        protected virtual void OnUpdateFrame(Camera camera, FrameEventArgs args) { }
        protected virtual void OnRenderFrame(Camera camera, FrameEventArgs args) { }
        protected virtual void OnUnload(Camera camera) { }
    }
}
