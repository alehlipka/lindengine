using Lindengine.Core;
using Demo.Scenes;
using OpenTK.Mathematics;

namespace Demo;

internal static class Program
{
    private static void Main()
    {
        Vector2i windowSize = new(800, 600);
        Lind.Engine.CreateWindow(windowSize, "Lindengine Demo", Path.Combine("Assets", "icon.png"));
        Lind.Engine.Scenes.Add(new DemoScene("main", windowSize));
        Lind.Engine.Scenes.Select("main");

        Lind.Engine.Run();
    }
}