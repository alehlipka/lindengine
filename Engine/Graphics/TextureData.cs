using Lindengine.Utilities;
using OpenTK.Mathematics;

namespace Lindengine.Graphics;

/// <param name="data">Image file bytes</param>
/// <param name="size">Image size</param>
public class TextureData(byte[] data, Vector2i size)
{
    /// <summary>
    /// Original image file data
    /// </summary>
    public readonly byte[] OriginalData = data;
    /// <summary>
    /// Vertically flipped image file data
    /// </summary>
    public readonly byte[] FlippedData = UtilityFunctions.GetVerticalFlippedBitmap(data, size);
    /// <summary>
    /// Image size
    /// </summary>
    public Vector2i Size = size;
}