using lindengine.common.logs;
using lindengine.core.window;

namespace lindengine.core
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Logger.Write(LogLevel.First, "Application start", true);

            using MainWindow mainWindow = new();
            mainWindow.Run();

            Logger.Write(LogLevel.Last, "Application finish");
        }
    }
}
