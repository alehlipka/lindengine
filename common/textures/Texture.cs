using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using StbImageSharp;

namespace lindengine.common.textures
{
    public class Texture(string name, int glHandle, int width, int height, TextureUnit unit = TextureUnit.Texture0)
    {
        public readonly string Name = name;
        public readonly int Handle = glHandle;
        public int Width = width;
        public int Height = height;
        public readonly TextureUnit Unit = unit;

        public static Texture LoadFromBytes(string name, byte[] bytes, Vector2i size, bool mipmap = false, TextureUnit unit = TextureUnit.Texture0)
        {
            int handle = GL.GenTexture();

            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, handle);

            bytes = FlipPixelsVertically(bytes, size);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, size.X, size.Y, 0, PixelFormat.Rgba, PixelType.UnsignedByte, bytes);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            if (mipmap) GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            GL.BindTexture(TextureTarget.Texture2D, 0);

            return new Texture(name, handle, size.X, size.Y, unit);
        }

        public void Change(byte[] bytes, Vector2i size)
        {
            bytes = FlipPixelsVertically(bytes, size);

            Width = size.X;
            Height = size.Y;

            GL.BindTexture(TextureTarget.Texture2D, Handle);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, size.X, size.Y, 0, PixelFormat.Rgba, PixelType.UnsignedByte, bytes);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public static Texture LoadFromFile(string name, string path, bool mipmap = false, TextureUnit unit = TextureUnit.Texture0)
        {
            byte[] data = File.ReadAllBytes(path);
            ImageResult image = ImageResult.FromMemory(data, ColorComponents.RedGreenBlueAlpha);

            data = image.Data;
            Vector2i size = new(image.Width, image.Height);
            Texture texture = LoadFromBytes(name, data, size, mipmap, unit);

            return texture;
        }

        private static byte[] FlipPixelsVertically(byte[] frameData, Vector2i size, int pixelSize = 4)
        {
            byte[] data = new byte[frameData.Length];
            for (int k = 0; k < size.Y; k++)
            {
                int j = size.Y - k - 1;
                System.Buffer.BlockCopy(
                    frameData, k * size.X * pixelSize,
                    data, j * size.X * pixelSize,
                    size.X * pixelSize);
            }

            return data;
        }

        public void Use()
        {
            GL.ActiveTexture(Unit);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }
    }
}
