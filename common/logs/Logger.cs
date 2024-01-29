namespace lindengine.common.logs
{
    public static class Logger
    {
        private static char first = '┌';
        private static char starter = '├';
        private static char line = '─';
        private static char last = '└';
        private static char separator = '│';

        public static void Write(LogLevel logLevel, string text, bool withSeparator = false)
        {
            char startChar = starter;

            switch (logLevel)
            {
                case LogLevel.First:
                    startChar = first;
                    logLevel = LogLevel.Window;
                    break;
                case LogLevel.Last:
                    startChar = last;
                    logLevel = LogLevel.Window;
                    break;
            }

            int linesCount = (int)Math.Max(0, (int)logLevel);

            Console.WriteLine($"{startChar}{new string(line, linesCount)}[{text}]");

            if (withSeparator)
            {
                Console.WriteLine(separator);
            }
        }
    }
}
