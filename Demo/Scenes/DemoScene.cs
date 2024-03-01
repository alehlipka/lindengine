using Lindengine.Core;
using Lindengine.Graphics;
using Lindengine.Graphics.Shader;
using Lindengine.Input;
using Lindengine.Output.Camera;
using Lindengine.Scenes;
using Lindengine.UI;
using Lindengine.UI.Element;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Demo.Scenes;

public class DemoScene : Scene
{
    private readonly OrthographicCamera _orthographicCamera;
    private readonly Form _menuForm;

    public DemoScene(string name, Vector2i windowSize) : base(name, windowSize)
    {
        ShaderProgram shader = Lind.Engine.Resources.Load<ShaderProgram>(Path.Combine("Assets", "Shaders", "GUI"));
        _orthographicCamera = new OrthographicCamera();

        Texture panel1 = Lind.Engine.Resources.Load<Texture>(Path.Combine("Assets", "UI", "Panels", "panel_1.png"));
        Texture panel2 = Lind.Engine.Resources.Load<Texture>(Path.Combine("Assets", "UI", "Panels", "panel_2.png"));
        Texture panel3 = Lind.Engine.Resources.Load<Texture>(Path.Combine("Assets", "UI", "Panels", "panel_3.png"));
        Texture panel4 = Lind.Engine.Resources.Load<Texture>(Path.Combine("Assets", "UI", "Panels", "panel_4.png"));
        
        // Form insideForm1 = new(new Vector2i(350, 275), 32, ElementOrigin.BottomLeft, new Vector2(25, 100), panel1, shader);
        // Form insideForm2 = new(new Vector2i(350, 60), 12, ElementOrigin.BottomLeft, new Vector2(25, 25), panel2, shader);
        // Form insideInsideForm = new(new Vector2i(100, 100), 16, ElementOrigin.Center, new Vector2(350, 275)/2, panel3, shader);
        // insideForm1.AddElement(insideInsideForm);
        
        _menuForm = new Form(new Vector2i(400, 200), 16, ElementOrigin.BottomLeft, new Vector3(50, 100, 0), panel4, shader);
        // _menuForm.AddElement(insideForm1);
        // _menuForm.AddElement(insideForm2);
    }

    protected override void OnLoad()
    {
        _orthographicCamera.Load();
        _menuForm.Load();
    }

    protected override void OnWindowResize(Vector2i size)
    {
        _orthographicCamera.WindowResize(size);
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
        _menuForm.Render(_orthographicCamera, elapsedSeconds);
    }

    protected override void OnUnload()
    {
        _orthographicCamera.Unload();
        _menuForm.Unload();
    }
}