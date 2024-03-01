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
    private readonly int _unit;
    private byte[] _data = [];

    /// <summary>
    /// Texture constructor
    /// </summary>
    /// <param name="glHandle">OpenGL texture name</param>
    /// <param name="size">Texture size</param>
    /// <param name="unit">Texture unit</param>
    public Texture(int glHandle, Vector2i size, int unit = 0)
    {
        _handle = glHandle;
        Size = size;
        _unit = unit;
    }

    /// <param name="textureData"><see cref="Lindengine.Graphics.TextureData"/> object</param>
    /// <param name="unit">Texture unit</param>
    /// <param name="isVerticalFlip">Is bytes must be vertically flipped</param>
    public Texture(TextureData textureData, int unit = 0, bool isVerticalFlip = true)
    {
        Size = textureData.Size;
        _unit = unit;
        _data = isVerticalFlip ? textureData.FlippedData : textureData.OriginalData;
        
        GL.CreateTextures(TextureTarget.Texture2D, 1, out _handle);
        
        GL.TextureParameter(_handle, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TextureParameter(_handle, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);
        GL.TextureParameter(_handle, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TextureParameter(_handle, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        
        GL.TextureStorage2D(_handle, 1, SizedInternalFormat.Rgba16, Size.X, Size.Y);
        GL.TextureSubImage2D(_handle, 0, 0, 0, Size.X, Size.Y, PixelFormat.Rgba, PixelType.UnsignedByte, _data);
        GL.GenerateTextureMipmap(_handle);
    }

    /// <param name="textureData"><see cref="Lindengine.Graphics.TextureData"/> object</param>
    /// <param name="isVerticalFlip">Is bytes must be vertically flipped</param>
    public void UpdateData(TextureData textureData, bool isVerticalFlip = true)
    {
        Size = textureData.Size;
        _data = isVerticalFlip ? textureData.FlippedData : textureData.OriginalData;
        
        GL.TextureSubImage2D(_handle, 0, 0, 0, Size.X, Size.Y, PixelFormat.Rgba, PixelType.UnsignedByte, _data);
        GL.GenerateTextureMipmap(_handle);
    }

    /// <summary>
    /// Set texture as current target
    /// </summary>
    public void Use()
    {
        GL.BindTextureUnit(_unit, _handle);
    }

    public void Unload()
    {
        GL.DeleteTexture(_handle);
    }
}
