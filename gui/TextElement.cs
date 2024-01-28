using static StbTrueTypeSharp.StbTrueType;

namespace lindengine.gui
{
    public class TextElement : Element
    {
        public TextElement(string name, string text) : base(name)
        {
            test();
        }

        unsafe public static void test()
        {
            string s = "Привет";
            float fontSize = 32.0f;
            int bufferWidth = 150;
            int bufferHeight = 200;

            byte[] bytes = File.ReadAllBytes("/home/procrastinator/projects/lindengine/core/assets/fonts/DroidSans.ttf");
            byte[] buffer = new byte[bufferWidth * bufferHeight];

            stbtt_fontinfo info = new stbtt_fontinfo();

            fixed (byte* ptr = bytes)
            {
                int res = stbtt_InitFont(info, ptr, 0);
            }

            int ascent, descent, lineGap;
            stbtt_GetFontVMetrics(info, &ascent, &descent, &lineGap);
            int lineHeight = ascent - descent + lineGap;

            float scale = stbtt_ScaleForPixelHeight(info, fontSize);

            ascent = (int)(ascent * scale + 0.5f);
            descent = (int)(descent * scale - 0.5f);
            lineHeight = (int)(lineHeight * scale + 0.5f);

            int posX = 0, posY = 0;

            for (int i = 0; i < s.Length; ++i)
            {
                char c = s[i];
                int glyphId = stbtt_FindGlyphIndex(info, c);
                if (glyphId == 0)
                {
                    continue;
                }

                int advanceTemp = 0, lsbTemp = 0;
                stbtt_GetGlyphHMetrics(info, glyphId, &advanceTemp, &lsbTemp);
                int advance = (int)(advanceTemp * scale + 0.5f);

                int x0, y0, x1, y1;
                //  2, -20, 17, 0
                stbtt_GetGlyphBitmapBox(
                    info, glyphId, scale, scale, &x0, &y0, &x1, &y1
                );

                posX += x0;
                posY = ascent + y0;

                if (posY < 0)
                {
                    posY = 0;
                }

                fixed (byte* ptr = buffer)
                {
                    byte* ptr2 = ptr + (posX + posY * bufferWidth);

                    stbtt_MakeGlyphBitmap(
                        info,
                        ptr2,
                        x1 - x0,
                        y1 - y0,
                        bufferWidth,
                        scale,
                        scale,
                        glyphId
                    );
                }

                posX += advance;
            }

            var imageWriter = new StbImageWriteSharp.ImageWriter();
            using (var stream = File.OpenWrite("output.png"))
            {
                imageWriter.WritePng(buffer, bufferWidth, bufferHeight, StbImageWriteSharp.ColorComponents.Grey, stream);
            }
        }
    }
}
