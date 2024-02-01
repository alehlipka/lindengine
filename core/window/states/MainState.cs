using lindengine.core.helpers;
using lindengine.gui;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace lindengine.core.window.states
{
    internal class MainState(string name, Vector2i windowSize) : State(name, windowSize)
    {
        private readonly Text? textElement = new($"{name}_text_element", new Vector2i(windowSize.X, 36), string.Empty, 36, Color4.Black);
        private readonly Background? background = new($"{name}_background", windowSize, Path.Combine("assets", "backgrounds", "mainmenu.png"));

        protected override void OnLoad(State state)
        {
            background?.Load();
            textElement?.Load();
        }

        protected override void OnContextResize(State state, ResizeEventArgs args)
        {
            background?.Resize(args);
            textElement?.Resize(args);
        }

        protected override void OnUpdateFrame(State state, FrameEventArgs args)
        {
            textElement?.SetText($"FPS: {FPSCounter.FPS} Max: {FPSCounter.Max} Min: {FPSCounter.Min}");

            background?.Update(args);
            textElement?.Update(args);
        }

        protected override void OnRenderFrame(State state, FrameEventArgs args)
        {
            background?.Render(args);
            textElement?.Render(args);
        }

        protected override void OnUnload(State state)
        {
            background?.Unload();
            textElement?.Unload();
        }
    }
}
