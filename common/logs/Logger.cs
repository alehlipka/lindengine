using System.Diagnostics;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

namespace lindengine.common.logs
{
    public static class Logger
    {
        private static readonly char first = '┌';
        private static readonly char starter = '├';
        private static readonly char line = '─';
        private static readonly char last = '└';
        private static readonly char separator = '│';
        private static readonly char openBox = '[';
        private static readonly char closeBox = ']';
        private static readonly int timeSize = 13;
        private static readonly double timeHigh = 1;
        private static readonly double timeMid = 0.5;
        private static readonly string titleOrder = "order".ToUpper();
        private static readonly string titleTime = "time".ToUpper();
        private static readonly string titleMessage = "message".ToUpper();
        private static readonly ConsoleColor textColor = ConsoleColor.White;
        private static readonly ConsoleColor lineColor = ConsoleColor.DarkCyan;
        private static readonly ConsoleColor timeLowColor = ConsoleColor.Green;
        private static readonly ConsoleColor timeMidColor = ConsoleColor.Yellow;
        private static readonly ConsoleColor timeHighColor = ConsoleColor.Red;

        private static int Top = 0;
        private static int fpsCounter = 0;

        private static TimeSpan totalTime;
        private static TimeSpan lastTime;

        public static void Write(LogLevel logLevel, string text, bool withSeparator = false)
        {
            totalTime = DateTime.Now - Process.GetCurrentProcess().StartTime;
            int linesCount = (int)Math.Max(0, (int)logLevel);
            double elapsedTime = (totalTime - lastTime).TotalSeconds;
            int logLevelsCount = Enum.GetNames(typeof(LogLevel)).Length;

            if (logLevel == LogLevel.First)
            {
                lastTime = totalTime;
                DrawHeader(linesCount + (logLevelsCount - linesCount) + 3);
            }

            DrawStartChar(starter);
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

            lastTime = totalTime;

            if (logLevel == LogLevel.Last)
            {
                DrawFooter();
            }
        }

        private static void DrawFooter()
        {
            DrawStartChar(last);
            DrawOpenBox();
            DrawText($"{totalTime.TotalSeconds}");
            DrawFinalBox();
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
            Console.ForegroundColor = textColor;
            Console.Write(text);
            Console.ResetColor();
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
