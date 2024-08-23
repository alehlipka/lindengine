using Lindengine.Framework.Mangers;
using Lindengine.Framework.Scenes;
using Lindengine.Framework.Windowing;

namespace Lindengine.Framework;

public sealed class Lind
{
    private static readonly Lazy<Lind> Lazy = new(() => new Lind(new SceneManager()));
    public static Lind Engine => Lazy.Value;

    private readonly Window _window;
    private readonly SceneManager _sceneManager;

    private Lind(SceneManager sceneManager)
    {
        _sceneManager = sceneManager;
        _window = new Window();
    }

    public Lind AddScene(string name, Scene scene)
    {
        _sceneManager.AddScene(name, scene);
        return this;
    }
    
    public Scene GetCurrentScene()
    {
        return _sceneManager.GetCurrentScene();
    }

    public Lind SelectScene(string name)
    {
        _sceneManager.SelectScene(name);
        return this;
    }

    public void Run()
    {
        _window.Run();
    }
}