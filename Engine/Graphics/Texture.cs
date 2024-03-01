using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Lindengine.Graphics;

public class Texture
{
    /// <summary>
    /// Texture size
    /// </summary>
    public Vector2i Size { get; private set; }
    
    private readonly int _handle;
    private readonly TextureUnit _unit;
    private byte[] _data = [];

    /// <summary>
    /// Texture constructor
    /// </summary>
    /// <param name="glHandle">OpenGL texture name</param>
    /// <param name="size">Texture size</param>
    /// <param name="unit">Texture unit</param>
    public Texture(int glHandle, Vector2i size, TextureUnit unit)
    {
        _handle = glHandle;
        Size = size;
        _unit = unit;
    }

    /// <param name="textureData"><see cref="Lindengine.Graphics.TextureData"/> object</param>
    /// <param name="unit">Texture unit</param>
    /// <param name="isVerticalFlip">Is bytes must be vertically flipped</param>
    public Texture(TextureData textureData, TextureUnit unit, bool isVerticalFlip = true)
    {
        Size = textureData.Size;
        _unit = unit;
        _data = isVerticalFlip ? textureData.FlippedData : textureData.OriginalData;
        
        _handle = GL.GenTexture();
        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, _handle);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Size.X, Size.Y, 0, PixelFormat.Rgba, PixelType.UnsignedByte, _data);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        GL.BindTexture(TextureTarget.Texture2D, 0);
    }

    /// <param name="textureData"><see cref="Lindengine.Graphics.TextureData"/> object</param>
    /// <param name="isVerticalFlip">Is bytes must be vertically flipped</param>
    public void UpdateData(TextureData textureData, bool isVerticalFlip = true)
    {
        Size = textureData.Size;
        _data = isVerticalFlip ? textureData.FlippedData : textureData.OriginalData;
        
        GL.BindTexture(TextureTarget.Texture2D, _handle);
        GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, Size.X, Size.Y, PixelFormat.Rgba, PixelType.UnsignedByte, _data);
        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        GL.BindTexture(TextureTarget.Texture2D, 0);
    }

    /// <summary>
    /// Set texture as current target
    /// </summary>
    public void Use()
    {
        GL.BindTexture(TextureTarget.Texture2D, _handle);
    }
}
