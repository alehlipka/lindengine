using OpenTK.Mathematics;

namespace Lindengine.Core.Interfaces;

public interface IManagedItem
{
    public void Load();
    public void Resize(Vector2i size, bool force = false);
    public void Update(double elapsedSeconds, bool force = false);
    public void Render(double elapsedSeconds, bool force = false);
    public void Unload();
}
