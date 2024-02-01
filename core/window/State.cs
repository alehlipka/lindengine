using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace lindengine.core.window
{
    internal class State
    {
        public readonly string Name;
        public delegate void StateDelegate(State sender);
        public event StateDelegate? UnloadedEvent;

        protected Vector2i windowSize;

        private bool _isLoaded;

        private delegate void StateContextResizeDelegate(State state, ResizeEventArgs args);
        private delegate void StateFrameDelegate(State state, FrameEventArgs args);

        private event StateDelegate? LoadEvent;
        private event StateDelegate? UnloadEvent;
        private event StateContextResizeDelegate? ContextResizeEvent;
        private event StateFrameDelegate? UpdateEvent;
        private event StateFrameDelegate? RendereEvent;

        public State(string name, Vector2i windowSize)
        {
            _isLoaded = false;

            Name = name.ToLower();
            this.windowSize = windowSize;
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

        public void Resize(ResizeEventArgs e)
        {
            if (_isLoaded)
            {
                windowSize = new Vector2i(e.Width, e.Height);
                ContextResizeEvent?.Invoke(this, e);
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
                UnloadedEvent?.Invoke(this);
            }
        }

        protected virtual void OnLoad(State state) { }
        protected virtual void OnContextResize(State state, ResizeEventArgs args) { }
        protected virtual void OnUpdateFrame(State state, FrameEventArgs args) { }
        protected virtual void OnRenderFrame(State state, FrameEventArgs args) { }
        protected virtual void OnUnload(State state) { }
    }
}
