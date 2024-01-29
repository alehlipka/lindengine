using lindengine.common.logs;
using OpenTK.Windowing.Common;

namespace lindengine.core.window
{
    internal class State
    {
        public readonly string Name;
        public delegate void StateDelegate(State sender);
        public event StateDelegate? UnloadedEvent;

        private bool _isLoaded;

        private delegate void StateContextResizeDelegate(State state, ResizeEventArgs args);
        private delegate void StateFrameDelegate(State state, FrameEventArgs args);

        private event StateDelegate? CreateEvent;
        private event StateDelegate? LoadEvent;
        private event StateDelegate? UnloadEvent;
        private event StateContextResizeDelegate? ContextResizeEvent;
        private event StateFrameDelegate? UpdateEvent;
        private event StateFrameDelegate? RendereEvent;

        public State(string name)
        {
            Logger.Write(LogLevel.State, $"State creating: {name}");
            
            _isLoaded = false;

            Name = name.ToLower();

            CreateEvent += OnCreate;
            CreateEvent?.Invoke(this);

            Logger.Write(LogLevel.State, $"State created: {Name}");
        }

        public void Load()
        {
            if (!_isLoaded)
            {
                Logger.Write(LogLevel.State, $"State loading: {Name}");

                LoadEvent += OnLoad;
                ContextResizeEvent += OnContextResize;
                UpdateEvent += OnUpdateFrame;
                RendereEvent += OnRenderFrame;
                UnloadEvent += OnUnload;

                LoadEvent?.Invoke(this);
                _isLoaded = true;

                Logger.Write(LogLevel.State, $"State loaded: {Name}");
            }
        }

        public void Resize(ResizeEventArgs e)
        {
            if (_isLoaded)
            {
                Logger.Write(LogLevel.State, $"State context resizing: {Name} {e.Width}x{e.Height}");

                ContextResizeEvent?.Invoke(this, e);

                Logger.Write(LogLevel.State, $"State context resized: {Name} {e.Width}x{e.Height}");
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
                Logger.Write(LogLevel.State, $"State unloading: {Name}");

                UnloadEvent?.Invoke(this);

                CreateEvent -= OnCreate;
                LoadEvent -= OnLoad;
                ContextResizeEvent -= OnContextResize;
                UpdateEvent -= OnUpdateFrame;
                RendereEvent -= OnRenderFrame;
                UnloadEvent -= OnUnload;

                _isLoaded = false;
                UnloadedEvent?.Invoke(this);

                Logger.Write(LogLevel.State, $"State unloaded: {Name}");
            }
        }

        protected virtual void OnCreate(State state) { }
        protected virtual void OnLoad(State state) { }
        protected virtual void OnContextResize(State state, ResizeEventArgs args) { }
        protected virtual void OnUpdateFrame(State state, FrameEventArgs args) { }
        protected virtual void OnRenderFrame(State state, FrameEventArgs args) { }
        protected virtual void OnUnload(State state) { }
    }
}
