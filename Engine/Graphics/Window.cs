using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Lindengine.Graphics;

internal class Window : GameWindow
{
    public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {

    }

    protected override void OnLoad()
    {
        GL.Enable(EnableCap.CullFace);
        GL.Enable(EnableCap.Multisample);
        GL.Enable(EnableCap.Blend);

        GL.CullFace(CullFaceMode.Back);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        GL.ClearColor(Color4.Black);
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        GL.Viewport(0, 0, e.Width, e.Height);
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);

        SwapBuffers();
    }

    protected override void OnUnload()
    {

    }
}
