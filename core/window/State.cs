using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace lindengine.core.window
{
    internal class State(string name, Vector2i windowSize)
    {
        public readonly string Name = name.ToLower();
        public bool IsLoaded { get; protected set; } = false;
        public Vector2i WindowSize { get; protected set; } = windowSize;

        private delegate void StateDelegate(State sender);
        private delegate void StateContextResizeDelegate(State state, ResizeEventArgs args);
        private delegate void StateFrameDelegate(State state, FrameEventArgs args);
        private event StateDelegate? LoadEvent;
        private event StateDelegate? UnloadEvent;
        private event StateContextResizeDelegate? ContextResizeEvent;
        private event StateFrameDelegate? UpdateEvent;
        private event StateFrameDelegate? RendereEvent;

        public void Load()
        {
            if (!IsLoaded)
            {
                LoadEvent += OnLoad;
                ContextResizeEvent += OnContextResize;
                UpdateEvent += OnUpdateFrame;
                RendereEvent += OnRenderFrame;
                UnloadEvent += OnUnload;

                LoadEvent?.Invoke(this);
                IsLoaded = true;
            }
        }

        public void Resize(ResizeEventArgs e)
        {
            if (IsLoaded)
            {
                WindowSize = new Vector2i(e.Width, e.Height);
                ContextResizeEvent?.Invoke(this, e);
            }
        }

        public void Update(FrameEventArgs args)
        {
            if (IsLoaded)
            {
                UpdateEvent?.Invoke(this, args);
            }
        }

        public void Render(FrameEventArgs args)
        {
            if (IsLoaded)
            {
                RendereEvent?.Invoke(this, args);
            }
        }

        public void Unload()
        {
            if (IsLoaded)
            {
                UnloadEvent?.Invoke(this);

                LoadEvent -= OnLoad;
                ContextResizeEvent -= OnContextResize;
                UpdateEvent -= OnUpdateFrame;
                RendereEvent -= OnRenderFrame;
                UnloadEvent -= OnUnload;

                IsLoaded = false;
            }
        }

        protected virtual void OnLoad(State state) { }
        protected virtual void OnContextResize(State state, ResizeEventArgs args) { }
        protected virtual void OnUpdateFrame(State state, FrameEventArgs args) { }
        protected virtual void OnRenderFrame(State state, FrameEventArgs args) { }
        protected virtual void OnUnload(State state) { }
    }
}
