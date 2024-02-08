using Lindengine.Core.Interfaces;
using OpenTK.Mathematics;
namespace Lindengine.Scenes;

public class Scene(string name, Vector2i size) : IManagedItem
{
    public readonly string Name = name;
    public Vector2i Size { get; protected set; } = size;
    public bool IsLoaded { get; protected set; } = false;

    private delegate void VoidDelegate();
    private delegate void ContextResizeDelegate(Vector2i size);
    private delegate void SecondsDelegate(double elapsedSeconds);
    private event VoidDelegate? LoadEvent;
    private event VoidDelegate? UnloadEvent;
    private event ContextResizeDelegate? ContextResizeEvent;
    private event SecondsDelegate? UpdateEvent;
    private event SecondsDelegate? RendereEvent;

    public void Load()
    {
        if (IsLoaded)
        {
            LoadEvent += OnLoad;
            ContextResizeEvent += OnContextResize;
            UpdateEvent += OnUpdateFrame;
            RendereEvent += OnRenderFrame;
            UnloadEvent += OnUnload;

            LoadEvent?.Invoke();

            IsLoaded = true;
        }
    }

    public void Resize(Vector2i size, bool force = false)
    {
        if (IsLoaded)
        {
            ContextResizeEvent?.Invoke(size);
        }
    }

    public void Update(double elapsedSeconds, bool force = false)
    {
        if (IsLoaded)
        {
            UpdateEvent?.Invoke(elapsedSeconds);
        }
    }

    public void Render(double elapsedSeconds, bool force = false)
    {
        if (IsLoaded)
        {
            RendereEvent?.Invoke(elapsedSeconds);
        }
    }

    public void Unload()
    {
        if (IsLoaded)
        {
            LoadEvent -= OnLoad;
            ContextResizeEvent -= OnContextResize;
            UpdateEvent -= OnUpdateFrame;
            RendereEvent -= OnRenderFrame;
            UnloadEvent -= OnUnload;

            UnloadEvent?.Invoke();

            IsLoaded = false;
        }
    }

    protected virtual void OnLoad() { }
    protected virtual void OnContextResize(Vector2i size) { }
    protected virtual void OnUpdateFrame(double elapsedSeconds) { }
    protected virtual void OnRenderFrame(double elapsedSeconds) { }
    protected virtual void OnUnload() { }
}
