using lindengine.gui;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace lindengine.core.window.states
{
    internal class MainState(string name) : State(name)
    {
        private TextElement? testElement;

        protected override void OnCreate(State state)
        {
            string text = "Test\n!\"#$%&'()*+,-./:;<=>?@[]^_`{|}~\n\nТекст(от лат. textus — ткань; сплетение, сочетание) — зафиксированная на каком-либо материальном носителе человеческая мысль; в общем плане связная и полная последовательность символов.";
            testElement = new TextElement("text_element", new Vector2i(280, 280), text);
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
