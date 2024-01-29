namespace lindengine.gui.font
{
    public static class FontsManager
    {
        private static string _fontsPath = string.Empty;
        private static readonly List<string> _fonts = [];

        public static void Create(string fontsPath)
        {
            _fontsPath = fontsPath;
        }
    }
}
