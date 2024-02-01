using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace lindengine.common.cameras
{
    public static class CameraManager
    {
        private static readonly List<Camera> _cameras = [];
        private static Camera? _selectedCamera = null;

        public static void Create(Vector2i windowSize)
        {
            _cameras.Clear();
            float aspect = windowSize.X / (float)windowSize.Y;
            _cameras.Add(new OrthographicCamera(CameraType.Orthographic, Vector3.UnitZ, Vector3.Zero, Vector3.UnitY, MathHelper.PiOver4, aspect, -100.0f, 100.0f));
        }

        public static void Select(CameraType type)
        {
            if (_selectedCamera?.Type != type)
            {
                _selectedCamera = _cameras.First(camera => camera.Type.Equals(type));
            }
        }

        public static Matrix4 GetViewMatrix()
        {
            return _selectedCamera?.ViewMatrix ?? Matrix4.Identity;
        }

        public static Matrix4 GetProjectionMatrix()
        {
            return _selectedCamera?.ProjectionMatrix ?? Matrix4.Identity;
        }

        public static void Load()
        {
            _cameras.ForEach(camera => camera.Load());
        }

        public static void Resize(ResizeEventArgs args)
        {
            _cameras.ForEach(camera => camera.Resize(args));
        }

        public static void Update(FrameEventArgs args)
        {
            _cameras.ForEach(camera => camera.Update(args));
        }

        public static void Render(FrameEventArgs args)
        {
            _cameras.ForEach(camera => camera.Render(args));
        }

        public static void Unload()
        {
            _cameras.ForEach(camera => camera.Unload());
        }
    }
}
