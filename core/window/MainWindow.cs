using lindengine.core.helpers;
using lindengine.shader;
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
            StartVisible = true,
            Vsync = VSyncMode.On
        };

        private const string consoleStarter = "├ ";

        public MainWindow() : base(_gameWindowSettings, _nativeWindowSettings)
        {
            Console.WriteLine("┌ Window creating");

            ShaderManager.Create(Path.Combine("assets", "shaders"));
            StatesManager.Create();

            Console.WriteLine(consoleStarter + "Window created");
            Console.WriteLine("│");
        }

        protected override void OnLoad()
        {
            Console.WriteLine(consoleStarter + "Window loading");

            GL.Enable(EnableCap.Multisample);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.ClearColor(Color4.Black);

            StatesManager.Load("main");

            Console.WriteLine(consoleStarter + "Window loaded");
            Console.WriteLine("│");
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            Console.WriteLine(consoleStarter + $"Window resizing: {e.Width}x{e.Height}");

            GL.Viewport(0, 0, e.Width, e.Height);

            StatesManager.Resize(e);

            Console.WriteLine(consoleStarter + $"Window resized: {e.Width}x{e.Height}");
            Console.WriteLine("│");
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
            Console.WriteLine(consoleStarter + "Window unloading");

            StatesManager.Unload();
            ShaderManager.Unload();

            Console.WriteLine("└ Window unloaded");
        }
    }
}
