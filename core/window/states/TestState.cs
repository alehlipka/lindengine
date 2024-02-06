using lindengine.gui;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace lindengine.core.window.states
{
    internal class TestState(string name, Vector2i windowSize) : State(name, windowSize)
    {
        private readonly Background? background = new($"{name}_background", Path.Combine("assets", "backgrounds", "forest.png"));

        protected override void OnLoad(State state)
        {
            background?.Load(WindowSize);
        }

        protected override void OnContextResize(State state, ResizeEventArgs args)
        {
            background?.Resize(args);
        }

        protected override void OnUpdateFrame(State state, FrameEventArgs args)
        {
            background?.Update(args);
        }

        protected override void OnRenderFrame(State state, FrameEventArgs args)
        {
            background?.Render(args);
        }

        protected override void OnUnload(State state)
        {
            background?.Unload();
        }
    }
}
