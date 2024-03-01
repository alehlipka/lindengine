using Lindengine.Utilities;
using OpenTK.Mathematics;

namespace Lindengine.Scenes;

/// <summary>
/// Game scene
/// </summary>
/// <param name="name"></param>
public abstract class Scene(string name, Vector2i windowSize)
{
    /// <summary>
    /// Scene name
    /// </summary>
    public readonly string Name = name;

    /// <summary>
    /// Scene (window) size
    /// </summary>
    public Vector2i Size { get; private set; } = windowSize;
    /// <summary>
    /// Is scene loaded and ready for actions
    /// </summary>
    public bool IsLoaded { get; protected set; }

    private event VoidDelegate? LoadEvent;
    private event VoidDelegate? UnloadEvent;
    private event SizeDelegate? WindowResizeEvent;
    private event SecondsDelegate? UpdateEvent;
    private event SecondsDelegate? RenderEvent;

    internal void Load()
    {
        if (IsLoaded) return;

        LoadEvent += OnLoad;
        WindowResizeEvent += OnWindowResize;
        UpdateEvent += OnUpdate;
        RenderEvent += OnRender;
        UnloadEvent += OnUnload;

        LoadEvent?.Invoke();

        IsLoaded = true;
    }

    internal void WindowResize(Vector2i size, bool force = false)
    {
        Size = size;
        if (!IsLoaded && !force) return;

        WindowResizeEvent?.Invoke(size);
    }

    internal void Update(double elapsedSeconds, bool force = false)
    {
        if (!IsLoaded && !force) return;

        UpdateEvent?.Invoke(elapsedSeconds);
    }

    internal void Render(double elapsedSeconds, bool force = false)
    {
        if (!IsLoaded && !force) return;

        RenderEvent?.Invoke(elapsedSeconds);
    }

    internal void Unload()
    {
        if (!IsLoaded) return;

        LoadEvent -= OnLoad;
        WindowResizeEvent -= OnWindowResize;
        UpdateEvent -= OnUpdate;
        RenderEvent -= OnRender;
        UnloadEvent?.Invoke();
        UnloadEvent -= OnUnload;

        IsLoaded = false;
    }

    protected abstract void OnLoad();
    protected abstract void OnWindowResize(Vector2i size);
    protected abstract void OnUpdate(double elapsedSeconds);
    protected abstract void OnRender(double elapsedSeconds);
    protected abstract void OnUnload();
}
