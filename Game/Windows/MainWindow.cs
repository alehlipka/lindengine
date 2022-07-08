using LindEngine.Core.Windows;
using OpenTK.Windowing.Desktop;

namespace LindEngine.Game.Windows;

public class MainWindow : LindenWindow
{
    public MainWindow(string name, GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(name, gameWindowSettings, nativeWindowSettings)
    {
        
    }
}