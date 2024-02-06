using lindengine.gui;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace lindengine.core.window.states
{
    internal class TestState(string name, Vector2i windowSize) : State(name, windowSize)
    {
        private readonly Background? background = new($"{name}_background", Path.Combine("assets", "backgrounds", "forest.png"));

        protected override void OnLoad()
        {
            background?.Load(WindowSize);
        }

        protected override void OnContextResize(ResizeEventArgs args)
        {
            background?.Resize(args);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            background?.Update(args);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            background?.Render(args);
        }

        protected override void OnUnload()
        {
            background?.Unload();
        }
    }
}
