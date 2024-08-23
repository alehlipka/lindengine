using Lindengine.Framework;

namespace Lindengine.Demo;

class Program
{
    private static void Main(string[] args)
    {
        Lind.Engine
            .AddScene("demo_scene", new DemoScene())
            .SelectScene("demo_scene")
            .Run();
    }
}