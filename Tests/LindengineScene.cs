using Lindengine.Scenes;
using OpenTK.Mathematics;

namespace Tests;

internal class TestScene(string name, Vector2i windowSize) : Scene(name, windowSize)
{
    protected override void OnLoad() { }
    protected override void OnWindowResize(Vector2i size) { }
    protected override void OnUpdate(double elapsedSeconds) { }
    protected override void OnRender(double elapsedSeconds) { }
    protected override void OnUnload() { }
}

[TestClass]
public class LindengineScene
{
    private readonly TestScene _scene = new("test scene", new Vector2i(800, 600));

    [TestMethod]
    public void IsSceneIsLoadedFalse()
    {
        Assert.IsFalse(_scene.IsLoaded, "IsLoaded should not be true");
    }

    [TestMethod]
    public void IsSceneNameNotNull()
    {
        Assert.IsNotNull(_scene.Name);
    }

    [TestMethod]
    public void IsSceneNameString()
    {
        Assert.IsInstanceOfType<string>(_scene.Name);
    }

    [TestMethod]
    public void IsSceneInstanceOfScene()
    {
        Assert.IsInstanceOfType<Scene>(_scene);
    }
}
