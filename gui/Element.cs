using lindengine.common.logs;
using lindengine.common.shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace lindengine.gui
{
    public class Element
    {
        public readonly string Name;

        protected int vertexBuffer;
        protected int indexBuffer;
        protected int vertexArray;
        protected uint[] indices;
        protected float[] vertices;
        protected List<Element> children = [];
        protected Vector2i size;
        protected Matrix4 modelMatrix = Matrix4.Identity;

        private delegate void ElementDelegate(Element element);
        private delegate void ElementContextResizeDelegate(Element element, ResizeEventArgs args);
        private delegate void ElementFrameDelegate(Element element, FrameEventArgs args);

        private event ElementDelegate? CreateEvent;
        private event ElementDelegate? LoadEvent;
        private event ElementDelegate? UnloadEvent;
        private event ElementContextResizeDelegate? ContextResizeEvent;
        private event ElementFrameDelegate? UpdateFrameEvent;
        private event ElementFrameDelegate? RendereFrameEvent;

        private bool _isLoaded;

        public Element(string name, Vector2i size)
        {
            Name = name.ToLower();
            this.size = size;

            CreateEvent += OnCreate;
            CreateEvent?.Invoke(this);

            Logger.Write(LogLevel.GUI, $"GUI element created: {Name}");
        }

        public void Load()
        {
            if (!_isLoaded)
            {
                LoadEvent += OnLoad;
                ContextResizeEvent += OnContextResize;
                UpdateFrameEvent += OnUpdateFrame;
                RendereFrameEvent += OnRenderFrame;
                UnloadEvent += OnUnload;

                LoadEvent?.Invoke(this);
                _isLoaded = true;

                Logger.Write(LogLevel.GUI, $"GUI element loaded: {Name}");
            }
        }

        public void Resize(ResizeEventArgs e)
        {
            if (_isLoaded)
            {
                ContextResizeEvent?.Invoke(this, e);

                Logger.Write(LogLevel.GUI, $"GUI element context resized: {Name} {e.Width}x{e.Height}");
            }
        }

        public void Update(FrameEventArgs args)
        {
            if (_isLoaded)
            {
                UpdateFrameEvent?.Invoke(this, args);
            }
        }

        public void Render(FrameEventArgs args)
        {
            if (_isLoaded)
            {
                RendereFrameEvent?.Invoke(this, args);
            }
        }

        public void Unload()
        {
            if (_isLoaded)
            {
                UnloadEvent?.Invoke(this);

                CreateEvent -= OnCreate;
                LoadEvent -= OnLoad;
                ContextResizeEvent -= OnContextResize;
                UpdateFrameEvent -= OnUpdateFrame;
                RendereFrameEvent -= OnRenderFrame;
                UnloadEvent -= OnUnload;

                _isLoaded = false;

                Logger.Write(LogLevel.GUI, $"GUI element unloaded: {Name}");
            }
        }

        protected virtual void OnCreate(Element element) { }
        protected virtual void OnLoad(Element element) { }
        protected virtual void OnContextResize(Element element, ResizeEventArgs args) { }
        protected virtual void OnUpdateFrame(Element element, FrameEventArgs args) { }
        protected virtual void OnRenderFrame(Element element, FrameEventArgs args) { }
        protected virtual void OnUnload(Element element) { }
    }
}
