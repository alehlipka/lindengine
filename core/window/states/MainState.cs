using lindengine.core.helpers;
using lindengine.gui;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace lindengine.core.window.states
{
    internal class MainState(string name, Vector2i windowSize) : State(name, windowSize)
    {
        private TextElement? testElement;

        protected override void OnCreate(State state)
        {
            testElement = new TextElement("text_element", windowSize, string.Empty);
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
            testElement?.SetText($"FPS: {FPSCounter.FPS} Max: {FPSCounter.Max} Min: {FPSCounter.Min}");
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
