using lindengine.common.logs;
using lindengine.common.shaders;
using lindengine.core.helpers;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace lindengine.core.window
{
    internal class MainWindow : GameWindow
    {
        private static readonly GameWindowSettings _gameWindowSettings = GameWindowSettings.Default;
        private static readonly NativeWindowSettings _nativeWindowSettings = new()
        {
            Title = "Lindengine",
            ClientSize = new Vector2i(800, 600),
            StartVisible = false,
            Vsync = VSyncMode.On
        };

        public MainWindow() : base(_gameWindowSettings, _nativeWindowSettings)
        {
            Logger.Write(LogLevel.First, "Window creating");

            ShaderManager.Create(Path.Combine("assets", "shaders"));
            StatesManager.Create();

            Logger.Write(LogLevel.Window, "Window created", true);
        }

        protected override void OnLoad()
        {
            Logger.Write(LogLevel.Window, "Window loading");

            GL.Enable(EnableCap.Multisample);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.ClearColor(Color4.Black);

            StatesManager.Load("main");

            Logger.Write(LogLevel.Window, "Window loaded", true);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            Logger.Write(LogLevel.Window, $"Window resizing: {e.Width}x{e.Height}");

            GL.Viewport(0, 0, e.Width, e.Height);

            StatesManager.Resize(e);

            Logger.Write(LogLevel.Window, $"Window resized: {e.Width}x{e.Height}", true);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Escape))
            {
                Close();
            }

            StatesManager.Update(args);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            FPSCounter.Calculate(args.Time);
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);

            StatesManager.Render(args);

            SwapBuffers();
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            Logger.Write(LogLevel.Window, "Window unloading");

            StatesManager.Unload();
            ShaderManager.Unload();

            Logger.Write(LogLevel.Last, "Window unloaded");
        }
    }
}
