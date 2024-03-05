using Demo.UI;
using Lindengine.Core;
using Lindengine.Graphics.Shader;
using Lindengine.Input;
using Lindengine.Output.Camera;
using Lindengine.Scenes;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Demo.Scenes;

public class DemoScene : Scene
{
    private readonly OrthographicCamera _orthographicCamera;
    private readonly MenuForm _menuForm;
    private readonly Background _background;
    private readonly Pointer _pointer;
    private readonly InfoText _infoText;

    public DemoScene(string name, Vector2i windowSize) : base(name, windowSize)
    {
        ShaderProgram shader = Lind.Engine.Resources.Load<ShaderProgram>(Path.Combine("Assets", "Shaders", "GUI"));
        _orthographicCamera = new OrthographicCamera();
        
        _menuForm = new MenuForm(new Vector2i(300, 200), shader);
        _background = new Background(Size, shader);
        _pointer = new Pointer(new Vector2i(23, 31), shader);
        _infoText = new InfoText(Size, shader);
    }

    protected override void OnLoad()
    {
        InputManager.SetCursorState(CursorState.Hidden);
        
        _orthographicCamera.Load();
        _menuForm.Load();
        _background.Load();
        _pointer.Load();
        _infoText.Load();
    }

    protected override void OnWindowResize(Vector2i size)
    {
        _orthographicCamera.WindowResize(size);
        _menuForm.WindowResize(size);
        _background.WindowResize(size);
        _pointer.WindowResize(size);
        _infoText.WindowResize(size);
    }

    protected override void OnUpdate(double elapsedSeconds)
    {
        _menuForm.Update(elapsedSeconds);
        _pointer.Update(elapsedSeconds);
        _infoText.Update(elapsedSeconds);

        if (InputManager.IsKeyboardKeyPressed(Keys.Escape))
        {
            Lind.Engine.Close();
        }
        else if (InputManager.IsKeyboardKeyPressed(Keys.Q))
        {
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
        }
        else if (InputManager.IsKeyboardKeyPressed(Keys.E))
        {
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
        }
        else if (InputManager.IsKeyboardKeyPressed(Keys.F))
        {
            Lind.Engine.ToggleFullScreen();
        }
        else if (InputManager.IsKeyboardKeyPressed(Keys.Z))
        {
            bool isDebug = !_background.IsDebug;
            _background.IsDebug = isDebug;
            _menuForm.IsDebug = isDebug;
            _pointer.IsDebug = isDebug;
        }
    }

    protected override void OnRender(double elapsedSeconds)
    {
        _orthographicCamera.Render(elapsedSeconds);
        GL.Disable(EnableCap.DepthTest);
        _background.Render(_orthographicCamera, elapsedSeconds);
        _infoText.Render(_orthographicCamera, elapsedSeconds);
        _menuForm.Render(_orthographicCamera, elapsedSeconds);
        _pointer.Render(_orthographicCamera, elapsedSeconds);
        GL.Enable(EnableCap.DepthTest);
    }

    protected override void OnUnload()
    {
        _orthographicCamera.Unload();
        _menuForm.Unload();
        _background.Unload();
        _pointer.Unload();
        _infoText.Unload();
    }
}