using Lindengine.Utilities;
using OpenTK.Mathematics;

namespace Lindengine.Output.Camera;

public abstract class Camera()
{
    public Vector2i Size { get; protected set; }
    public bool IsLoaded { get; protected set; }
    public Matrix4 ViewMatrix { get; protected set; } = Matrix4.Identity;
    public Matrix4 ProjectionMatrix { get; protected set; } = Matrix4.Identity;

    protected Vector3 Position;
    protected Vector3 Target;
    protected Vector3 Up;
    protected float Fov;
    protected float AspectRatio;
    public float NearClip { get; protected init; }
    public float FarClip { get; protected init; }
    
    private event VoidDelegate? LoadEvent;
    private event VoidDelegate? UnloadEvent;
    private event SizeDelegate? WindowResizeEvent;
    private event SecondsDelegate? UpdateEvent;
    private event SecondsDelegate? RenderEvent;
    
    public void Load()
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

    public void WindowResize(Vector2i size, bool force = false)
    {
        if (!IsLoaded && !force) return;

        Size = size;

        WindowResizeEvent?.Invoke(size);
    }

    public void Update(double elapsedSeconds, bool force = false)
    {
        if (!IsLoaded && !force) return;

        UpdateEvent?.Invoke(elapsedSeconds);
    }

    public void Render(double elapsedSeconds, bool force = false)
    {
        if (!IsLoaded && !force) return;

        RenderEvent?.Invoke(elapsedSeconds);
    }

    public void Unload()
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