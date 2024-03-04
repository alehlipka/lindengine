using Demo.UI;
using Lindengine.Core;
using Lindengine.Graphics;
using Lindengine.Graphics.Shader;
using Lindengine.Input;
using Lindengine.Output.Camera;
using Lindengine.Scenes;
using Lindengine.UI;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Demo.Scenes;

public class DemoScene : Scene
{
    private readonly OrthographicCamera _orthographicCamera;
    private readonly MenuForm _menuForm;
    private readonly Background _background;

    public DemoScene(string name, Vector2i windowSize) : base(name, windowSize)
    {
        ShaderProgram shader = Lind.Engine.Resources.Load<ShaderProgram>(Path.Combine("Assets", "Shaders", "GUI"));
        _orthographicCamera = new OrthographicCamera();
        
        _menuForm = new MenuForm(new Vector2i(400, 400), shader)
        {
            Origin = ElementOrigin.Center,
            Position = new Vector3(Size.X, Size.Y, 0) / 2,
            Border = 16,
            Texture = Lind.Engine.Resources.Load<Texture>(Path.Combine("Assets", "UI", "Panels", "panel_11.png"))
        };
        _background = new Background(Size, shader)
        {
            Texture = Lind.Engine.Resources.Load<Texture>(Path.Combine("Assets", "debug.jpg"))
        };
    }

    protected override void OnLoad()
    {
        _orthographicCamera.Load();
        _menuForm.Load();
        _background.Load();
    }

    protected override void OnWindowResize(Vector2i size)
    {
        _orthographicCamera.WindowResize(size);
        _menuForm.WindowResize(size);
        _background.WindowResize(size);
    }

    protected override void OnUpdate(double elapsedSeconds)
    {
        _menuForm.Update(elapsedSeconds);

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
    }

    protected override void OnRender(double elapsedSeconds)
    {
        _orthographicCamera.Render(elapsedSeconds);
        _background.Render(_orthographicCamera, elapsedSeconds);
        _menuForm.Render(_orthographicCamera, elapsedSeconds);
    }

    protected override void OnUnload()
    {
        _orthographicCamera.Unload();
        _menuForm.Unload();
        _background.Unload();
    }
}