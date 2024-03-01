using Lindengine.Graphics;
using Lindengine.Utilities;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common.Input;

namespace Lindengine.Resources.Loaders;

internal static class TextureLoader
{
    internal static Texture LoadResource(string path)
    {
        Image image = ImageLoader.LoadResource(path);
        byte[] data = UtilityFunctions.GetVerticalFlippedBitmap(image.Data, new Vector2i(image.Width, image.Height));
        
        GL.CreateTextures(TextureTarget.Texture2D, 1, out int handle);
        GL.TextureParameter(handle, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TextureParameter(handle, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);
        GL.TextureParameter(handle, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TextureParameter(handle, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        GL.TextureStorage2D(handle, 1, SizedInternalFormat.Rgba16, image.Width, image.Height);
        GL.TextureSubImage2D(handle, 0, 0, 0, image.Width, image.Height, PixelFormat.Rgba, PixelType.UnsignedByte, data);
        GL.GenerateTextureMipmap(handle);

        return new Texture(handle, new Vector2i(image.Width, image.Height));
    }
}
