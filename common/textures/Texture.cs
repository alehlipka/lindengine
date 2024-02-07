using OpenTK.Graphics.OpenGL4;

namespace lindengine.common.textures
{
    public class Texture(string name, int glHandle, int width, int height, TextureUnit unit = TextureUnit.Texture0)
    {
        public readonly string Name = name;
        public readonly int Handle = glHandle;
        public int Width = width;
        public int Height = height;
        public readonly TextureUnit Unit = unit;

        public static Texture LoadFromBytes(string name, TextureData data, bool mipmap = false, TextureUnit unit = TextureUnit.Texture0)
        {
            int handle = GL.GenTexture();

            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, handle);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Size.X, data.Size.Y, 0, PixelFormat.Rgba, PixelType.UnsignedByte, data.VerticalFlippedBytes);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            if (mipmap) GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            GL.BindTexture(TextureTarget.Texture2D, 0);

            return new Texture(name, handle, data.Size.X, data.Size.Y, unit);
        }

        public void Change(TextureData data)
        {
            Width = data.Size.X;
            Height = data.Size.Y;

            GL.BindTexture(TextureTarget.Texture2D, Handle);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Size.X, data.Size.Y, 0, PixelFormat.Rgba, PixelType.UnsignedByte, data.VerticalFlippedBytes);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void Use()
        {
            GL.ActiveTexture(Unit);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }
    }
}
