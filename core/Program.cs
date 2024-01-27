using lindengine.core.window;

namespace lindengine.core
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using MainWindow mainWindow = new();

            mainWindow.Run();
        }
    }
}
