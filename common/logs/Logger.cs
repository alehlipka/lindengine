using System;
using System.Diagnostics;

namespace lindengine.common.logs
{
    public static class Logger
    {
        private static char first = '┌';
        private static char starter = '├';
        private static char line = '─';
        private static char last = '└';
        private static char separator = '│';
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

                DrawHeader(1 + linesCount + 1 + (logLevelsCount - linesCount) + 1);
            }

            DrawStartChar(startChar);
            DrawLines(linesCount);
            DrawPosition();
            DrawLines(logLevelsCount - linesCount);
            DrawOpenBox();
            DrawElapsedTime(elapsedTime);
            DrawCloseBox();

            DrawLines(11 - elapsedTime.ToString().Length);

            DrawOpenBox();
            DrawText(text);
            DrawFinalBox();
            if (withSeparator) DrawSeparator();

            lastTime = time;
        }

        private static void DrawHeader(int count)
        {
            Console.WriteLine();
            Console.ForegroundColor = lineColor;
            Console.Write($"{first}{new string('─', count - 2)}");
            Console.Write("[time]");
            Console.Write($"{new string('─', 11 - "time".Length)}");
            Console.WriteLine("[message]");
            Console.WriteLine(separator);
            Console.ResetColor();
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
            Console.Write('[');
            Console.ResetColor();
        }

        private static void DrawCloseBox()
        {
            Console.ForegroundColor = lineColor;
            Console.Write(']');
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
            if (elapsedTime >= 1) Console.ForegroundColor = timeHighColor;
            else if (elapsedTime >= 0.5) Console.ForegroundColor = timeMidColor;
            else Console.ForegroundColor = timeLowColor;

            Console.Write(elapsedTime);
            Console.ResetColor();
        }

        private static void DrawFinalBox()
        {
            Console.ForegroundColor = lineColor;
            Console.WriteLine(']');
            Console.ResetColor();
        }
    }
}
