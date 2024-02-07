using OpenTK.Mathematics;
using StbImageSharp;

namespace lindengine.common.textures
{
    public class TextureData(byte[] bytes, Vector2i size)
    {
        public byte[] Bytes = bytes;
        public Vector2i Size = size;

        public byte[] VerticalFlippedBytes
        {
            get
            {
                int pixelSize = 4;
                byte[] data = new byte[Bytes.Length];

                for (int k = 0; k < Size.Y; k++)
                {
                    int j = Size.Y - k - 1;
                    int srcOffset = k * Size.X * pixelSize;
                    int dstOffset = j * Size.X * pixelSize;
                    int count = Size.X * pixelSize;

                    Buffer.BlockCopy(Bytes, srcOffset, data, dstOffset, count);
                }

                return data;
            }
        }

        public static TextureData FromFile(string path)
        {
            byte[] data = File.ReadAllBytes(path);
            ImageResult image = ImageResult.FromMemory(data, ColorComponents.RedGreenBlueAlpha);

            return new TextureData(image.Data, new Vector2i(image.Width, image.Height));
        }
    }
}
