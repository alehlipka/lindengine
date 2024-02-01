using OpenTK.Mathematics;

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

        public static byte[] GetFileBytes(string fontName)
        {
            return _fonts.First(font => font.Name.Equals(fontName.ToLower()))?.FileBytes ?? [];
        }

        public static byte[] GetBitmapBytes(string fontName, Vector2i bitmapSize, string text, int fontSize)
        {
            return _fonts.First(font => font.Name.Equals(fontName.ToLower()))?.GetBitmapBytes(bitmapSize, text, fontSize) ?? [];
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
