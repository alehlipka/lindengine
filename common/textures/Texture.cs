using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using StbImageSharp;

namespace lindengine.common.textures
{
    public class Texture(string name, int glHandle, int width, int height, TextureUnit unit = TextureUnit.Texture0)
    {
        public readonly string Name = name;
        public readonly int Handle = glHandle;
        public readonly int Width = width;
        public readonly int Height = height;
        public readonly TextureUnit Unit = unit;

        public static Texture LoadFromBytes(string name, byte[] bytes, Vector2i size, TextureUnit unit = TextureUnit.Texture0)
        {
            int handle = GL.GenTexture();

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, handle);

            bytes = FlipPixelsVertically(bytes, size);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, size.X, size.Y, 0, PixelFormat.Rgba, PixelType.UnsignedByte, bytes);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            return new Texture(name, handle, size.X, size.Y, unit);
        }

        public static Texture LoadFromFile(string name, string path, TextureUnit unit = TextureUnit.Texture0)
        {
            ImageResult image;
            using (Stream stream = File.OpenRead(path))
            {
                image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
            }

            return LoadFromBytes(name, image.Data, new Vector2i(image.Width, image.Height), unit);
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
