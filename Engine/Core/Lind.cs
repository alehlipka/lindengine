using Lindengine.Resources;
using Lindengine.Scenes;
using Lindengine.Utilities;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.Desktop;

namespace Lindengine.Core;

/// <summary>
/// Singleton framework class
/// </summary>
public sealed class Lind
{
    private static readonly Lazy<Lind> Lazy = new(() => new Lind());
    public static Lind Engine => Lazy.Value;
    public SceneManager Scenes { get; } = new();
    public ResourceLoader Resources { get; } = new();

    internal readonly EventManager Events = new();
    internal Window? Window;

    private Lind() { }

    /// <summary>
    /// Create game window
    /// </summary>
    /// <param name="size">Window size</param>
    /// <param name="title">Window title</param>
    /// <param name="iconPath">Path to icon</param>
    /// <param name="updateFrequency">Update frequency in hertz</param>
    public void CreateWindow(Vector2i size, string title = "Lindengine", string? iconPath = null, double updateFrequency = 60)
    {
        GameWindowSettings gameWindowSettings = new()
        {
            UpdateFrequency = updateFrequency
        };
        NativeWindowSettings nativeWindowSettings = new()
        {
            Title = title,
            ClientSize = new Vector2i(size.X, size.Y),
            Vsync = VSyncMode.Off,
            WindowBorder = WindowBorder.Resizable,
            IsEventDriven = false,
            API = ContextAPI.OpenGL,
            APIVersion = new Version(4, 6),
            Profile = ContextProfile.Core,
            NumberOfSamples = 8
        };

        if (iconPath != null)
        {
            Image image = Resources.Load<Image>(iconPath);
            nativeWindowSettings.Icon = new WindowIcon(image);
        }

        Window = new Window(gameWindowSettings, nativeWindowSettings);
    }

    /// <summary>
    /// Run game loop
    /// </summary>
    public void Run()
    {
        Window?.Run();
    }

    /// <summary>
    /// Close window
    /// </summary>
    public void Close()
    {
        Window?.Close();
    }
}