using System.Diagnostics;
using lindengine.core.helpers;
using lindengine.gui;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace lindengine.core.window.states
{
    internal class MainState(string name, Vector2i windowSize) : State(name, windowSize)
    {
        private readonly Text? textElement = new($"{name}_text_element", $"State: {name}", 16, Color4.Black);
        private readonly Background? background = new($"{name}_background", Path.Combine("assets", "backgrounds", "mainmenu.png"));

        protected override void OnLoad()
        {
            background?.Load(WindowSize);
            textElement?.Load(WindowSize);
        }

        protected override void OnContextResize(ResizeEventArgs args)
        {
            background?.Resize(args);
            textElement?.Resize(args);
        }

        double time;
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            time += args.Time;
            if (time >= 1)
            {
                float memoryUsed = Process.GetCurrentProcess().PrivateMemorySize64 / 1024f / 1024f;
                string text = $"State: {Name}\n" +
                    $"Used memory: {memoryUsed:0.000} MB\n" +
                    $"FPS: {FPSCounter.FPS}\n" +
                    $"Vendor: {GL.GetString(StringName.Vendor)}\n" +
                    $"Version: {GL.GetString(StringName.Version)}\n" +
                    $"Renderer: {GL.GetString(StringName.Renderer)}\n" +
                    $"GLSL compiler: {GL.GetString(StringName.ShadingLanguageVersion)}";
                textElement?.SetText(text);
            }

            background?.Update(args);
            textElement?.Update(args);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            background?.Render(args);
            textElement?.Render(args);
        }

        protected override void OnUnload()
        {
            background?.Unload();
            textElement?.Unload();
        }
    }
}
