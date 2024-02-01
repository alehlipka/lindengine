using lindengine.common.textures;
using OpenTK.Mathematics;
using static StbTrueTypeSharp.StbTrueType;

namespace lindengine.gui;

internal static class BitmapText
{
    private static byte[] bytes = [], bitmap = [];
    private static int l_h, b_cursor, ascent, descent, lineGap;
    private static float scale;
    private static stbtt_fontinfo info = new();
    private static int lineShift;
    private static string text = string.Empty;
    private static Vector2i imageSize;

    public static byte[] GetBytes(string fontPath, Vector2i imageSize, string text, int fontSize)
    {
        create(fontPath, imageSize, fontSize, text);

        // каждый пиксель содержит 4 байта (R, G, B, A)
        byte[] rgbaData = new byte[imageSize.X * imageSize.Y * 4];
        // Копирование данных из 8-битного изображения в буфер RGBA
        for (int y = 0; y < imageSize.Y; y++)
        {
            for (int x = 0; x < imageSize.X; x++)
            {
                // Получение значения яркости пикселя из исходного изображения
                byte intensity = bitmap[y * imageSize.X + x];

                // Установка значений компонентов RGBA в буфере
                int index = (y * imageSize.X + x) * 4;
                rgbaData[index] = intensity; // Красный
                rgbaData[index + 1] = intensity; // Зелёный
                rgbaData[index + 2] = intensity; // Синий
                rgbaData[index + 3] = intensity > 0 ? (byte)255 : (byte)0; // Непрозрачность (значение 255 означает полностью непрозрачный пиксель)
            }
        }

        return rgbaData;
    }

    public static byte[] GetBytes(byte[] fontBytes, Vector2i imageSize, string text, int fontSize)
    {
        create(fontBytes, imageSize, fontSize, text);

        // каждый пиксель содержит 4 байта (R, G, B, A)
        byte[] rgbaData = new byte[imageSize.X * imageSize.Y * 4];
        // Копирование данных из 8-битного изображения в буфер RGBA
        for (int y = 0; y < imageSize.Y; y++)
        {
            for (int x = 0; x < imageSize.X; x++)
            {
                // Получение значения яркости пикселя из исходного изображения
                byte intensity = bitmap[y * imageSize.X + x];

                // Установка значений компонентов RGBA в буфере
                int index = (y * imageSize.X + x) * 4;
                rgbaData[index] = intensity; // Красный
                rgbaData[index + 1] = intensity; // Зелёный
                rgbaData[index + 2] = intensity; // Синий
                rgbaData[index + 3] = intensity > 0 ? (byte)255 : (byte)0; // Непрозрачность (значение 255 означает полностью непрозрачный пиксель)
            }
        }

        return rgbaData;
    }

    unsafe private static void create(string fontPath, Vector2i imageSize, int fontSize, string text)
    {
        create(File.ReadAllBytes(fontPath), imageSize, fontSize, text);
    }

    unsafe private static void create(byte[] fontBytes, Vector2i imageSize, int fontSize, string text)
    {
        BitmapText.imageSize = imageSize;
        BitmapText.text = text;

        bytes = fontBytes;
        info = new();
        fixed (byte* ptr = bytes)
        {
            stbtt_InitFont(info, ptr, 0);
        }

        l_h = fontSize;

        bitmap = new byte[imageSize.X * imageSize.Y];
        scale = stbtt_ScaleForPixelHeight(info, l_h);
        b_cursor = 0;
        int asc, des, gap;
        stbtt_GetFontVMetrics(info, &asc, &des, &gap);
        ascent = (int)(asc * scale);
        descent = (int)(des * scale);
        lineGap = gap;

        lineShift = 0;

        processText();
    }

    unsafe static private int getTextWidth(string text)
    {
        int wordWidth = 0;

        int ax, lsb;
        foreach (char character in text)
        {
            stbtt_GetCodepointHMetrics(info, character, &ax, &lsb);
            wordWidth += (int)(ax * scale);
        }

        return wordWidth;
    }

    static private void processText()
    {
        string[] lines = text.Split('\n');
        for (int line_number = 0; line_number < lines.Length; line_number++)
        {
            processLine(lines[line_number], line_number);
        }
    }

    static private void processLine(string line, int line_number)
    {
        b_cursor = imageSize.X * l_h * (line_number + lineShift);

        string[] words = line.Split(' ');
        int currentLineWidth = 0;

        for (int word_number = 0; word_number < words.Length; word_number++)
        {
            int wordWidth = getTextWidth(words[word_number] + ' ');
            currentLineWidth += wordWidth;

            if (currentLineWidth >= imageSize.X)
            {
                lineShift++;
                b_cursor = imageSize.X * l_h * (line_number + lineShift);
                currentLineWidth = wordWidth;
            }

            processWord(words[word_number]);
        }
    }

    static private void processWord(string word)
    {
        for (int char_number = 0; char_number < word.Length; char_number++)
        {
            char current_char = word[char_number];
            char? next_char = (char_number < word.Length - 1)
                ? word[char_number + 1]
                : null;

            processCharacter(current_char, next_char);
            if (next_char == null)
            {
                processCharacter(' ');
            }
        }
    }

    unsafe static private void processCharacter(char current_char, char? next_char = null)
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
            int byteOffset = b_cursor + lsb + (y * imageSize.X);
            byte* ptr2 = ptr + byteOffset;
            stbtt_MakeCodepointBitmap(info, ptr2, char_width, char_height, imageSize.X, scale, scale, current_char);
        }

        /* advance x */
        b_cursor += ax;
    }
}
