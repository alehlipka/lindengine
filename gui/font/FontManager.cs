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

        public static byte[] GetBitmapBytes(string fontName, Vector2i bitmapSize, string text, int fontSize, Color4 color)
        {
            Font? font = _fonts.Find(font => font.Name.Equals(fontName.ToLower()));
            byte[]? bytes = font?.GetBitmapBytes(bitmapSize, text, fontSize, color);

            return bytes ?? [];
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
