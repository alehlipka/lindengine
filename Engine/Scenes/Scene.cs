using OpenTK.Mathematics;
namespace Lindengine.Scenes;

internal class Scene(string name, Vector2i size)
{
    public readonly string Name = name;
    public Vector2i Size = size;
    public bool IsLoaded = false;

    public void Load()
    {
        if (IsLoaded)
        {
            IsLoaded = true;
        }
    }

    public void Resize()
    {
        if (IsLoaded)
        {
        }
    }

    public void Update()
    {
        if (IsLoaded)
        {
        }
    }

    public void Render()
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
