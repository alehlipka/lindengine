using lindengine.common.cameras;
using lindengine.common.shaders;
using lindengine.core.helpers;
using lindengine.gui.font;
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
            Vsync = VSyncMode.On
        };

        public MainWindow() : base(_gameWindowSettings, _nativeWindowSettings)
        {
            CameraManager.Create(ClientSize);
            ShaderManager.Create(Path.Combine("assets", "shaders"));
            FontManager.Create(Path.Combine("assets", "fonts"));
            StatesManager.Create(ClientSize);
        }

        protected override void OnLoad()
        {
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.Multisample);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.ClearColor(Color4.Black);

            CameraManager.Load();
            StatesManager.Load("main", ClientSize);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);

            CameraManager.Resize(e);
            StatesManager.Resize(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            if (IsKeyPressed(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Escape))
            {
                Close();
            }
            else if (IsKeyPressed(OpenTK.Windowing.GraphicsLibraryFramework.Keys.F))
            {
                if (IsFullscreen)
                {
                    WindowState = WindowState.Normal;
                }
                else
                {
                    WindowState = WindowState.Fullscreen;
                }
            }
            else if (IsKeyPressed(OpenTK.Windowing.GraphicsLibraryFramework.Keys.D1))
            {
                StatesManager.Load("main", ClientSize);
            }
            else if (IsKeyPressed(OpenTK.Windowing.GraphicsLibraryFramework.Keys.D2))
            {
                StatesManager.Load("test", ClientSize);
            }

            CameraManager.Update(args);
            StatesManager.Update(args);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            FPSCounter.Calculate(args.Time);
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);

            CameraManager.Render(args);
            StatesManager.Render(args);

            SwapBuffers();
        }

        protected override void OnUnload()
        {
            StatesManager.Unload();
            ShaderManager.Unload();
            CameraManager.Unload();
        }
    }
}
