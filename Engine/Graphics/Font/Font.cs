namespace Lindengine.Graphics.Font;

/// <param name="bytes">TTF file bytes</param>
public class Font(byte[] bytes)
{
    public readonly byte[] FileBytes = bytes;
}
