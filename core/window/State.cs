using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

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

        private const string consoleStarter = "├──── ";

        public State(string name)
        {
            Console.WriteLine(consoleStarter + $"State creating: {name}");
            _isLoaded = false;

            Name = name.ToLower();

            CreateEvent += OnCreate;
            CreateEvent?.Invoke(this);
            Console.WriteLine(consoleStarter + $"State created: {Name}");
        }

        public void Load()
        {
            if (!_isLoaded)
            {
                Console.WriteLine(consoleStarter + $"State loading: {Name}");
                LoadEvent += OnLoad;
                ContextResizeEvent += OnContextResize;
                UpdateEvent += OnUpdateFrame;
                RendereEvent += OnRenderFrame;
                UnloadEvent += OnUnload;

                LoadEvent?.Invoke(this);
                _isLoaded = true;
                Console.WriteLine(consoleStarter + $"State loaded: {Name}");
            }
        }

        public void Resize(ResizeEventArgs e)
        {
            if (_isLoaded)
            {
                Console.WriteLine(consoleStarter + $"State context resizing: {Name} {e.Width}x{e.Height}");
                ContextResizeEvent?.Invoke(this, e);
                Console.WriteLine(consoleStarter + $"State context resized: {Name} {e.Width}x{e.Height}");
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
                Console.WriteLine(consoleStarter + $"State unloading: {Name}");
                UnloadEvent?.Invoke(this);

                CreateEvent -= OnCreate;
                LoadEvent -= OnLoad;
                ContextResizeEvent -= OnContextResize;
                UpdateEvent -= OnUpdateFrame;
                RendereEvent -= OnRenderFrame;
                UnloadEvent -= OnUnload;

                _isLoaded = false;
                UnloadedEvent?.Invoke(this);
                Console.WriteLine(consoleStarter + $"State unloaded: {Name}");
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
