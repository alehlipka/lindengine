using OpenTK.Mathematics;

namespace Lindengine.Core;

public interface IManager<T>
{
    public void Add(T item);
    public void AddMany(IEnumerable<T> items);

    public void Remove(T item, bool withUnloading = true);
    public void Remove(string name, bool withUnloading = true);
    public void RemoveMany(IEnumerable<T> items, bool withUnloading = true);

    public T? Select(string name);

    public void Resize(Vector2i size);
    public void Update(double elapsedSeconds);
    public void Render(double elapsedSeconds);
    public void Unload();

    public void ResizeAll(Vector2i size);
    public void UpdateAll(double elapsedSeconds);
    public void RenderAll(double elapsedSeconds);
    public void UnloadAll();
}
