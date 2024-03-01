using Lindengine.Utilities;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Lindengine.Core;

internal class Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
    : GameWindow(gameWindowSettings, nativeWindowSettings)
{
    protected override void OnLoad()
    {
        GL.Enable(EnableCap.CullFace);
        GL.Enable(EnableCap.Multisample);
        GL.Enable(EnableCap.Blend);
        GL.CullFace(CullFaceMode.Back);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        GL.ClearColor(Color4.Black);
        Lind.Engine.Events.TriggerEvent(EventTarget.Window, "load");
        Lind.Engine.Scenes.LoadAll();
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        GL.Viewport(0, 0, e.Width, e.Height);
        Lind.Engine.Events.TriggerEvent(EventTarget.Window, "resize");
        Lind.Engine.Scenes.ResizeAll(new Vector2i(e.Width, e.Height));
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        Lind.Engine.Events.TriggerEvent(EventTarget.Window, "update");
        Lind.Engine.Scenes.UpdateSelected(args.Time);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
        Lind.Engine.Events.TriggerEvent(EventTarget.Window, "render");
        Lind.Engine.Scenes.RenderSelected(args.Time);
        SwapBuffers();
    }

    protected override void OnUnload()
    {
        Lind.Engine.Events.TriggerEvent(EventTarget.Window, "unload");
        Lind.Engine.Scenes.UnloadAll();
    }
}
