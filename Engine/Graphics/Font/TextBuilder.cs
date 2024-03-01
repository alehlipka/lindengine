using Lindengine.Utilities;
using OpenTK.Mathematics;
using static StbTrueTypeSharp.StbTrueType;

namespace Lindengine.Graphics.Font;

public sealed class TextBuilder(string text, Font font, int fontSize, Color4 color, Vector2i drawSize, bool dots = true)
{
    private stbtt_fontinfo _fontInfo = new();
    private float _scale;
    private int _bitmapCursor, _lineNumber, _ascent;
    private byte[] _bitmapBytes = [];
    private const char NewLineCharacter = '\n';

    public unsafe TextureData GetTextureData()
    {
        _fontInfo = new stbtt_fontinfo();
        fixed (byte* ptr = font.FileBytes) stbtt_InitFont(_fontInfo, ptr, 0);
        _scale = stbtt_ScaleForPixelHeight(_fontInfo, fontSize);
        _bitmapCursor = 0;
        int asc, desc, gap;
        stbtt_GetFontVMetrics(_fontInfo, &asc, &desc, &gap);
        _ascent = (int)(asc * _scale);
        _lineNumber = 0;
        _bitmapBytes = new byte[drawSize.X * drawSize.Y];
        ProcessText();
        
        return new TextureData(UtilityFunctions.MonochromeToRgba(_bitmapBytes, drawSize, color) , drawSize);
    }

    private bool NewLine()
    {
        _lineNumber++;
        _bitmapCursor = drawSize.X * fontSize * _lineNumber;

        return (_lineNumber + 1) * fontSize <= drawSize.Y;
    }

    private void ProcessText()
    {
        int dotsWidth = dots ? GetCharacterWidth('.') * 3 : 0;
        string[] lines = text.Split(NewLineCharacter);
        foreach (string line in lines)
        {
            int charactersWidth = 0;
            foreach (char currentChar in line)
            {
                int charWidth = GetCharacterWidth(currentChar);
                if (charactersWidth + charWidth + dotsWidth >= drawSize.X)
                {
                    if (dots)
                    {
                        ProcessCharacter('.');
                        ProcessCharacter('.');
                        ProcessCharacter('.');
                    }
                    break;
                }
            
                ProcessCharacter(currentChar);
                charactersWidth += charWidth;
            }
            
            if (!NewLine()) return;
        }
    }
    
    private unsafe void ProcessCharacter(char currentChar)
    {
        int ax, lsb;
        stbtt_GetCodepointHMetrics(_fontInfo, currentChar, &ax, &lsb);
        ax = (int)(ax * _scale);
        lsb = (int)(lsb * _scale);int cX1, cY1, cX2, cY2;
        stbtt_GetCodepointBitmapBox(_fontInfo, currentChar, _scale, _scale, &cX1, &cY1, &cX2, &cY2);
        int charWidth = cX2 - cX1;
        int charHeight = cY2 - cY1;
        int y = _ascent + cY1;
        fixed (byte* ptr = _bitmapBytes)
        {
            int byteOffset = _bitmapCursor + lsb + (y * drawSize.X);
            byte* ptr2 = ptr + byteOffset;
            stbtt_MakeCodepointBitmap(_fontInfo, ptr2, charWidth, charHeight, drawSize.X, _scale, _scale, currentChar);
        }
        _bitmapCursor += ax;
    }
    
    private unsafe int GetCharacterWidth(char targetCharacter)
    {
        int ax, lsb;
        stbtt_GetCodepointHMetrics(_fontInfo, targetCharacter, &ax, &lsb);
        return (int)(ax * _scale);
    }
}
