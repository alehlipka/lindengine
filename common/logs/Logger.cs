using System.Diagnostics;
using System.Reflection;

namespace lindengine.common.logs
{
    public static class Logger
    {
        private static char first = '┌';
        private static char starter = '├';
        private static char line = '─';
        private static char last = '└';
        private static char separator = '│';
        private static char openBox = '[';
        private static char closeBox = ']';
        private static int timeSize = 13;
        private static double timeHigh = 1;
        private static double timeMid = 0.5;
        private static string titleOrder = "order".ToUpper();
        private static string titleTime = "time".ToUpper();
        private static string titleMessage = "message".ToUpper();
        private static ConsoleColor lineColor = ConsoleColor.DarkCyan;
        private static ConsoleColor timeLowColor = ConsoleColor.Green;
        private static ConsoleColor timeMidColor = ConsoleColor.Yellow;
        private static ConsoleColor timeHighColor = ConsoleColor.Red;

        private static TimeSpan lastTime;

        public static void Write(LogLevel logLevel, string text, bool withSeparator = false)
        {
            char startChar = starter;

            switch (logLevel)
            {
                case LogLevel.First:
                    logLevel = LogLevel.Application;
                    //startChar = first;
                    break;
                case LogLevel.Last:
                    startChar = last;
                    logLevel = LogLevel.Application;
                    break;
            }

            TimeSpan time = DateTime.Now - Process.GetCurrentProcess().StartTime;
            int linesCount = (int)Math.Max(0, (int)logLevel);
            double elapsedTime = (time - lastTime).TotalSeconds;
            int logLevelsCount = Enum.GetNames(typeof(LogLevel)).Length;

            if (lastTime.TotalMicroseconds == 0)
            {
                lastTime = time;
                DrawHeader(linesCount + (logLevelsCount - linesCount) + 3);
            }

            DrawStartChar(startChar);
            DrawLines(linesCount);
            DrawPosition();
            DrawLines(logLevelsCount - linesCount);
            DrawOpenBox();
            DrawElapsedTime(elapsedTime);
            DrawCloseBox();
            DrawLines(timeSize - elapsedTime.ToString().Length);
            DrawOpenBox();
            DrawText(text);
            DrawFinalBox();
            if (withSeparator) DrawSeparator();

            lastTime = time;
        }

        private static void DrawHeader(int count)
        {
            string version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "0.0.0.0";

            Console.Clear();
            Console.WriteLine();

            DrawStartChar(first);
            DrawOpenBox();
            DrawText($"Lindengine framework".ToUpper());
            DrawCloseBox();
            DrawLines(1);
            DrawOpenBox();
            DrawText($"v.{version}");
            DrawFinalBox();

            DrawStartChar(starter);
            DrawOpenBox();
            DrawText(titleOrder);
            DrawCloseBox();
            DrawLines(count - titleOrder.Length - 4);
            DrawOpenBox();
            DrawText(titleTime);
            DrawCloseBox();
            DrawLines(timeSize - titleTime.Length);
            DrawOpenBox();
            DrawText(titleMessage);
            DrawFinalBox();

            DrawSeparator();
        }

        private static void DrawStartChar(char startChar)
        {
            Console.ForegroundColor = lineColor;
            Console.Write(startChar);
            Console.ResetColor();
        }

        private static void DrawPosition()
        {
            Console.ForegroundColor = lineColor;
            Console.Write("¤");
            Console.ResetColor();
        }

        private static void DrawLines(int linesCount)
        {
            Console.ForegroundColor = lineColor;
            Console.Write(new string(line, linesCount));
            Console.ResetColor();
        }

        private static void DrawText(string text)
        {
            Console.ResetColor();
            Console.Write(text);
        }

        private static void DrawOpenBox()
        {
            Console.ForegroundColor = lineColor;
            Console.Write(openBox);
            Console.ResetColor();
        }

        private static void DrawCloseBox()
        {
            Console.ForegroundColor = lineColor;
            Console.Write(closeBox);
            Console.ResetColor();
        }

        private static void DrawSeparator()
        {
            Console.ForegroundColor = lineColor;
            Console.WriteLine(separator);
            Console.ResetColor();
        }

        private static void DrawElapsedTime(double elapsedTime)
        {
            if (elapsedTime >= timeHigh) Console.ForegroundColor = timeHighColor;
            else if (elapsedTime >= timeMid) Console.ForegroundColor = timeMidColor;
            else Console.ForegroundColor = timeLowColor;

            Console.Write(elapsedTime);
            Console.ResetColor();
        }

        private static void DrawFinalBox()
        {
            Console.ForegroundColor = lineColor;
            Console.WriteLine(closeBox);
            Console.ResetColor();
        }
    }
}
