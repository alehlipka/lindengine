using lindengine.gui;
using OpenTK.Windowing.Common;

namespace lindengine.core.window.states
{
    internal class MainState(string name) : State(name)
    {
        private TextElement? testElement;

        protected override void OnCreate(State state)
        {
            testElement = new TextElement("test", "example text");
        }

        protected override void OnLoad(State state)
        {
            testElement?.Load();
        }

        protected override void OnContextResize(State state, ResizeEventArgs args)
        {
            testElement?.Resize(args);
        }

        protected override void OnUpdateFrame(State state, FrameEventArgs args)
        {
            testElement?.Update(args);
        }

        protected override void OnRenderFrame(State state, FrameEventArgs args)
        {
            testElement?.Render(args);
        }

        protected override void OnUnload(State state)
        {
            testElement?.Unload();
        }
    }
}
