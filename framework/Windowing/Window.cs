using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace lindengine.framework.Windowing;

public class Window : GameWindow
{
    public Window(string title = "Lindengine Demo", bool isVSyncEnabled = false)
        : base(GameWindowSettings.Default, new NativeWindowSettings()
        {
            APIVersion = new Version(4, 6),
            Title = title,
            Vsync = isVSyncEnabled ? VSyncMode.On : VSyncMode.Off
        })
    {
        
    }
}