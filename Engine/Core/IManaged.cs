using OpenTK.Mathematics;

namespace Lindengine;

public interface IManaged
{
    public void Load();
    public void Resize(Vector2i size);
    public void Update(double elapsedSeconds);
    public void Render(double elapsedSeconds);
    public void Unload();
}
