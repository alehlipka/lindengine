using LindEngine.Core.Managers;
using OpenTK.Windowing.Desktop;

namespace LindEngine.Core;

public class Application
{
    public static Application Starter { get; } = new();
    private Application() {}

    /// <summary>
    /// Start application instance
    /// </summary>
    public void Start()
    {
        Window.Manager.AddWindow("main", GameWindowSettings.Default,
            new NativeWindowSettings() { Title = "LindEngine" });
        Window.Manager.Run();
    }
}