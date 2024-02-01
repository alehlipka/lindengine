using OpenTK.Mathematics;
using static StbTrueTypeSharp.StbTrueType;

namespace lindengine.gui;

internal class Font(string name, byte[] bytes)
{
    public readonly string Name = name;
    public readonly byte[] FileBytes = bytes;

    private stbtt_fontinfo _fontinfo = new();
    private byte[] _bitmapBytes = [];
    private float _scale;
    private int _bitmapCursor, _lineShift, _fontSize;
    private int _ascent, _descent, _lineGap;
    private Vector2i _bitmapSize;

    unsafe public byte[] GetBitmapBytes(Vector2i bitmapSize, string text, int fontSize)
    {
        _fontinfo = new();
        fixed (byte* ptr = FileBytes) stbtt_InitFont(_fontinfo, ptr, 0);
        _fontSize = fontSize;
        _bitmapSize = bitmapSize;
        _bitmapBytes = new byte[_bitmapSize.X * _bitmapSize.Y];
        _scale = stbtt_ScaleForPixelHeight(_fontinfo, _fontSize);
        _bitmapCursor = 0;
        int asc, desc, gap;
        stbtt_GetFontVMetrics(_fontinfo, &asc, &desc, &gap);
        _ascent = (int)(asc * _scale);
        _descent = (int)(desc * _scale);
        _lineGap = gap;
        _lineShift = 0;

        ProcessText(text);

        return ToRGBA();
    }

    private byte[] ToRGBA()
    {
        byte[] rgbaData = new byte[_bitmapSize.X * _bitmapSize.Y * 4];
        for (int y = 0; y < _bitmapSize.Y; y++)
        {
            for (int x = 0; x < _bitmapSize.X; x++)
            {
                byte intensity = _bitmapBytes[y * _bitmapSize.X + x];

                int index = (y * _bitmapSize.X + x) * 4;
                rgbaData[index] = intensity;
                rgbaData[index + 1] = intensity;
                rgbaData[index + 2] = intensity;
                rgbaData[index + 3] = intensity > 0 ? (byte)255 : (byte)0;
            }
        }

        return rgbaData;
    }

    unsafe private int GetTextWidth(string text)
    {
        int wordWidth = 0;

        int ax, lsb;
        foreach (char character in text)
        {
            stbtt_GetCodepointHMetrics(_fontinfo, character, &ax, &lsb);
            wordWidth += (int)(ax * _scale);
        }

        return wordWidth;
    }

    private void ProcessText(string text)
    {
        string[] lines = text.Split('\n');
        for (int line_number = 0; line_number < lines.Length; line_number++)
        {
            ProcessLine(lines[line_number], line_number);
        }
    }

    private void ProcessLine(string line, int line_number)
    {
        _bitmapCursor = _bitmapSize.X * _fontSize * (line_number + _lineShift);

        string[] words = line.Split(' ');
        int currentLineWidth = 0;

        for (int word_number = 0; word_number < words.Length; word_number++)
        {
            int wordWidth = GetTextWidth(words[word_number] + ' ');
            currentLineWidth += wordWidth;

            if (currentLineWidth >= _bitmapSize.X)
            {
                _lineShift++;
                _bitmapCursor = _bitmapSize.X * _fontSize * (line_number + _lineShift);
                currentLineWidth = wordWidth;
            }

            ProcessWord(words[word_number]);
        }
    }

    private void ProcessWord(string word)
    {
        for (int char_number = 0; char_number < word.Length; char_number++)
        {
            char current_char = word[char_number];
            char? next_char = (char_number < word.Length - 1)
                ? word[char_number + 1]
                : null;

            ProcessCharacter(current_char, next_char);
            if (next_char == null)
            {
                ProcessCharacter(' ');
            }
        }
    }

    unsafe private void ProcessCharacter(char current_char, char? next_char = null)
    {
        /* how wide is this character */
        int ax, lsb;
        stbtt_GetCodepointHMetrics(_fontinfo, current_char, &ax, &lsb);
        ax = (int)(ax * _scale);
        lsb = (int)(lsb * _scale);
        /* (Note that each Codepoint call has an alternative Glyph version which caches the work required to lookup the character word[i].) */

        /* get bounding box for character (may be offset to account for chars that dip above or below the line) */
        int c_x1, c_y1, c_x2, c_y2;
        stbtt_GetCodepointBitmapBox(_fontinfo, current_char, _scale, _scale, &c_x1, &c_y1, &c_x2, &c_y2);
        int char_width = c_x2 - c_x1;
        int char_height = c_y2 - c_y1;

        /* compute y (different characters have different heights) */
        int y = _ascent + c_y1;

        fixed (byte* ptr = _bitmapBytes)
        {
            /* render character (stride and offset is important here) */
            int byteOffset = _bitmapCursor + lsb + (y * _bitmapSize.X);
            byte* ptr2 = ptr + byteOffset;
            stbtt_MakeCodepointBitmap(_fontinfo, ptr2, char_width, char_height, _bitmapSize.X, _scale, _scale, current_char);
        }

        /* advance x */
        _bitmapCursor += ax;
    }
}
