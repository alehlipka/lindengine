using Lindengine.Utilities;
using OpenTK.Mathematics;
using static StbTrueTypeSharp.StbTrueType;

namespace Lindengine.Graphics.Font;

public sealed class TextBuilder()
{
    private const char NewLineCharacter = '\n';
    private const bool Dots = true;
    
    private stbtt_fontinfo _fontInfo = new();
    private float _scale;
    private int _bitmapCursor, _lineNumber, _ascent;
    private byte[] _bitmapBytes = [];
    private int _maxLineWidth;
    private Vector2i _cursorShift;

    public unsafe TextureData Draw(string text, Font font, int fontSize, Color4 color, Vector2i drawSize, TextAlign textAlign)
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
        _cursorShift = Vector2i.Zero;
        GetMaxLineWidth(text, drawSize);

        if (textAlign == TextAlign.Center)
        {
            _cursorShift.X = (drawSize.X - _maxLineWidth) / 2;
            _bitmapCursor = _cursorShift.X;
        }
        
        ProcessText(text, fontSize, drawSize);
        
        return new TextureData(UtilityFunctions.MonochromeToRgba(_bitmapBytes, drawSize, color) , drawSize);
    }

    private void GetMaxLineWidth(string text, Vector2i drawSize)
    {
        foreach (string line in text.Split(NewLineCharacter))
        {
            int lineWidth = line.Sum(GetCharacterWidth);
            if (_maxLineWidth < lineWidth)
            {
                _maxLineWidth = lineWidth;
            }
        }

        _maxLineWidth = int.Min(drawSize.X, _maxLineWidth);
    }

    private bool NewLine(int fontSize, Vector2i drawSize)
    {
        _lineNumber++;
        _bitmapCursor = drawSize.X * fontSize * _lineNumber + _cursorShift.X;

        return (_lineNumber + 1) * fontSize <= drawSize.Y;
    }

    private void ProcessText(string text, int fontSize, Vector2i drawSize)
    {
        int dotsWidth = Dots ? GetCharacterWidth('.') * 3 : 0;
        string[] lines = text.Split(NewLineCharacter);
        foreach (string line in lines)
        {
            int charactersWidth = 0;
            foreach (char currentChar in line)
            {
                int charWidth = GetCharacterWidth(currentChar);
                if (charactersWidth + charWidth + dotsWidth >= drawSize.X)
                {
                    if (Dots)
                    {
                        ProcessCharacter('.', drawSize);
                        ProcessCharacter('.', drawSize);
                        ProcessCharacter('.', drawSize);
                    }
                    break;
                }
            
                ProcessCharacter(currentChar, drawSize);
                charactersWidth += charWidth;
            }
            
            if (!NewLine(fontSize, drawSize)) return;
        }
    }
    
    private unsafe void ProcessCharacter(char currentChar, Vector2i drawSize)
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
