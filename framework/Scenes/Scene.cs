namespace Lindengine.Framework.Scenes;

public abstract class Scene
{
    public abstract void Initialize();

    public abstract void Update(double time);
    
    public abstract void Render();
    
    public abstract void Dispose();
}