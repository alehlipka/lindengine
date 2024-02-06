using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace lindengine.core.window
{
    internal class State(string name, Vector2i windowSize)
    {
        public readonly string Name = name.ToLower();
        public bool IsLoaded { get; protected set; } = false;
        public Vector2i WindowSize { get; protected set; } = windowSize;

        private delegate void StateDelegate();
        private delegate void StateContextResizeDelegate(ResizeEventArgs args);
        private delegate void StateFrameDelegate(FrameEventArgs args);
        private event StateDelegate? LoadEvent;
        private event StateDelegate? UnloadEvent;
        private event StateContextResizeDelegate? ContextResizeEvent;
        private event StateFrameDelegate? UpdateEvent;
        private event StateFrameDelegate? RendereEvent;

        public void Load(Vector2i windowSize)
        {
            if (!IsLoaded)
            {
                LoadEvent += OnLoad;
                ContextResizeEvent += OnContextResize;
                UpdateEvent += OnUpdateFrame;
                RendereEvent += OnRenderFrame;
                UnloadEvent += OnUnload;

                WindowSize = windowSize;
                LoadEvent?.Invoke();
                IsLoaded = true;
            }
        }

        public void Resize(ResizeEventArgs e)
        {
            if (IsLoaded)
            {
                WindowSize = new Vector2i(e.Width, e.Height);
                ContextResizeEvent?.Invoke(e);
            }
        }

        public void Update(FrameEventArgs args)
        {
            if (IsLoaded)
            {
                UpdateEvent?.Invoke(args);
            }
        }

        public void Render(FrameEventArgs args)
        {
            if (IsLoaded)
            {
                RendereEvent?.Invoke(args);
            }
        }

        public void Unload()
        {
            if (IsLoaded)
            {
                UnloadEvent?.Invoke();

                LoadEvent -= OnLoad;
                ContextResizeEvent -= OnContextResize;
                UpdateEvent -= OnUpdateFrame;
                RendereEvent -= OnRenderFrame;
                UnloadEvent -= OnUnload;

                IsLoaded = false;
            }
        }

        protected virtual void OnLoad() { }
        protected virtual void OnContextResize(ResizeEventArgs args) { }
        protected virtual void OnUpdateFrame(FrameEventArgs args) { }
        protected virtual void OnRenderFrame(FrameEventArgs args) { }
        protected virtual void OnUnload() { }
    }
}
