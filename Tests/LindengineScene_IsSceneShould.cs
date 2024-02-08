using Lindengine.Scenes;

namespace Lindengine.UnitTests.Scenes;

[TestClass]
public class LindengineScene_IsSceneShould
{
    private readonly Scene _scene;

    public LindengineScene_IsSceneShould()
    {
        _scene = new Scene("test scene", new OpenTK.Mathematics.Vector2i(10, 10));
    }

    [TestMethod]
    public void IsScene_IsLoaded_ReturnFalse()
    {
        bool result = _scene.IsLoaded;

        Assert.IsFalse(result, "IsLoaded should not be true");
    }

    [TestMethod]
    public void IsScene_IsTypeOf_ReturnScene()
    {
        Assert.IsInstanceOfType(_scene, typeof(Scene));
    }
}
