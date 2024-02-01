namespace lindengine.gui.font
{
    public static class FontManager
    {
        private static string _fontsPath = string.Empty;
        private static readonly List<Font> _fonts = [];

        public static void Create(string fontsPath)
        {
            _fontsPath = fontsPath;
            CreateFonts();
        }

        public static byte[] GetBytes(string fontName)
        {
            return _fonts.First(font => font.Name.Equals(fontName.ToLower()))?.Bytes ?? [];
        }

        private static void CreateFonts()
        {
            _fonts.Clear();

            string[] files = Directory.GetFiles(_fontsPath);
            foreach (string file in files)
            {
                string fontName = Path.GetFileNameWithoutExtension(file).ToLower();
                _fonts.Add(new Font(fontName, File.ReadAllBytes(file)));
            }
        }
    }
}
