using OpenTK.Mathematics;
namespace Lindengine.Scenes;

public class Scene(string name, Vector2i size) : IManaged
{
    public readonly string Name = name;
    public Vector2i Size { get; protected set; } = size;
    public bool IsLoaded { get; protected set; } = false;

    public void Load()
    {
        if (IsLoaded)
        {
            IsLoaded = true;
        }
    }

    public void Resize(Vector2i size)
    {
        if (IsLoaded)
        {
        }
    }

    public void Update(double elapsedSeconds)
    {
        if (IsLoaded)
        {
        }
    }

    public void Render(double elapsedSeconds)
    {
        if (IsLoaded)
        {
        }
    }

    public void Unload()
    {
        if (IsLoaded)
        {
            IsLoaded = false;
        }
    }
}
