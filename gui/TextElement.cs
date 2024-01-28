using static StbTrueTypeSharp.StbTrueType;

namespace lindengine.gui
{
    public class TextElement : Element
    {
        byte[] bytes, bitmap;
        int b_w, b_h, l_h, b_cursor, ascent, descent, lineGap;
        float scale;
        stbtt_fontinfo info;

        public TextElement(string name, string text) : base(name)
        {
            text = "Пиздец\nSukablyat'\nЯ в полном ахуе от того как это работает!№(%*?)".Trim('\r');

            prepare();
            start(text);
        }

        unsafe public void prepare()
        {
            bytes = File.ReadAllBytes("/home/procrastinator/projects/lindengine/core/assets/fonts/DroidSans.ttf");
            info = new();
            fixed (byte* ptr = bytes)
            {
                stbtt_InitFont(info, ptr, 0);
            }

            b_w = 400;
            b_h = 300;
            l_h = 32;

            bitmap = new byte[b_w * b_h];
            scale = stbtt_ScaleForPixelHeight(info, l_h);
            b_cursor = 0;
            int asc, des, gap;
            stbtt_GetFontVMetrics(info, &asc, &des, &gap);
            ascent = (int)(asc * scale);
            descent = (int)(des * scale);
            lineGap = gap;
        }

        public void start(string text)
        {
            string[] lines = text.Split('\n');
            for (int line_number = 0; line_number < lines.Length; line_number++)
            {
                b_cursor = b_w * l_h * line_number;

                string[] words = lines[line_number].Split(' ');
                for (int word_number = 0; word_number < words.Length; word_number++)
                {
                    for (int char_number = 0; char_number < words[word_number].Length; char_number++)
                    {
                        char current_char = words[word_number][char_number];
                        char? next_char = (char_number < words[word_number].Length - 1)
                            ? words[word_number][char_number + 1]
                            : null;

                        proccessCharacter(current_char, next_char);
                        if (next_char == null)
                        {
                            proccessCharacter(' ');
                        }
                    }
                }
            }

            var imageWriter = new StbImageWriteSharp.ImageWriter();
            using var stream = File.OpenWrite("output.png");
            imageWriter.WritePng(bitmap, b_w, b_h, StbImageWriteSharp.ColorComponents.Grey, stream);
        }

        unsafe protected void proccessCharacter(char current_char, char? next_char = null)
        {
            /* how wide is this character */
            int ax, lsb;
            stbtt_GetCodepointHMetrics(info, current_char, &ax, &lsb);
            ax = (int)(ax * scale);
            lsb = (int)(lsb * scale);
            /* (Note that each Codepoint call has an alternative Glyph version which caches the work required to lookup the character word[i].) */

            /* get bounding box for character (may be offset to account for chars that dip above or below the line) */
            int c_x1, c_y1, c_x2, c_y2;
            stbtt_GetCodepointBitmapBox(info, current_char, scale, scale, &c_x1, &c_y1, &c_x2, &c_y2);
            int char_width = c_x2 - c_x1;
            int char_height = c_y2 - c_y1;

            /* compute y (different characters have different heights) */
            int y = ascent + c_y1;

            fixed (byte* ptr = bitmap)
            {
                /* render character (stride and offset is important here) */
                int byteOffset = b_cursor + lsb + (y * b_w);
                byte* ptr2 = ptr + byteOffset;
                stbtt_MakeCodepointBitmap(info, ptr2, char_width, char_height, b_w, scale, scale, current_char);
            }

            /* advance x */
            b_cursor += ax;

            /* add kerning */
            int kern = 0;
            if (next_char != null && next_char != '\n')
            {
                kern = stbtt_GetCodepointKernAdvance(info, current_char, (char)next_char);
                b_cursor += (int)(kern * scale);
            }
        }
    }
}
