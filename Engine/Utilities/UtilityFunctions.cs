using Lindengine.Core;
using Lindengine.Output.Camera;
using OpenTK.Mathematics;

namespace Lindengine.Utilities;

internal static class UtilityFunctions
{
    private const float Zero3 = 1.0f / 3.0f;
    private const float Zero6 = 1.0f - Zero3;
        
    /// <summary>
    /// Flip bitmap bytes vertically
    /// </summary>
    /// <param name="bytes">Bitmap bytes</param>
    /// <param name="size">Bitmap size</param>
    /// <param name="pixelSize">Pixel color elements count</param>
    /// <returns>Vertically flipped bitmap bytes array</returns>
    public static byte[] GetVerticalFlippedBitmap(byte[] bytes, Vector2i size, int pixelSize = 4)
    {
        byte[] data = new byte[bytes.Length];

        for (int i = 0; i < size.Y; i++)
        {
            int pointer = size.Y - i - 1;
            int srcOffset = i * size.X * pixelSize;
            int dstOffset = pointer * size.X * pixelSize;
            int count = size.X * pixelSize;

            Buffer.BlockCopy(bytes, srcOffset, data, dstOffset, count);
        }

        return data;
    }

    /// <summary>
    /// Create RGBA bitmap from monochrome one. Black - transparent, white - color
    /// </summary>
    /// <param name="bytes">Bitmap bytes</param>
    /// <param name="size">Bitmap size</param>
    /// <param name="color">New color for white</param>
    /// <returns>RGBA bitmap bytes</returns>
    public static byte[] MonochromeToRgba(byte[] bytes, Vector2i size, Color4 color)
    {
        byte[] rgbaData = new byte[size.X * size.Y * 4];
        for (int y = 0; y < size.Y; y++)
        {
            for (int x = 0; x < size.X; x++)
            {
                byte intensity = bytes[y * size.X + x];
                int index = (y * size.X + x) * 4;

                byte red = (byte)(color.R * 255);
                byte green = (byte)(color.G * 255);
                byte blue = (byte)(color.B * 255);
                byte alpha = (byte)(255 - (255 - intensity));

                rgbaData[index + 0] = red;
                rgbaData[index + 1] = green;
                rgbaData[index + 2] = blue;
                rgbaData[index + 3] = alpha;
            }
        }

        return rgbaData;
    }

    public static void GetBorderedVertices(Vector2i size, float border, out uint[] indices, out float[] vertices)
    {
        float[] contentVertices =
        [
            border, border, 0.0f, Zero3, Zero3, // bottom left
            border, size.Y - border, 0.0f, Zero3, Zero6, // top left
            size.X - border, size.Y - border, 0.0f, Zero6, Zero6, // top right
            size.X - border, border, 0.0f, Zero6, Zero3 // bottom right
        ];
        float[] bottomLeftBorderVertices =
        [
            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // bottom left
            0.0f, border, 0.0f, 0.0f, Zero3, // top left
            border, border, 0.0f, Zero3, Zero3, // top right
            border, 0.0f, 0.0f, Zero3, 0.0f // bottom right
        ];
        float[] centerLeftBorderVertices =
        [
            0.0f, border, 0.0f, 0.0f, Zero3, // bottom left
            0.0f, size.Y - border, 0.0f, 0.0f, Zero6, // top left
            border, size.Y - border, 0.0f, Zero3, Zero6, // top right
            border, border, 0.0f, Zero3, Zero3 // bottom right
        ];
        float[] topLeftBorderVertices =
        [
            0.0f, size.Y - border, 0.0f, 0.0f, Zero6, // bottom left
            0.0f, size.Y, 0.0f, 0.0f, 1.0f, // top left
            border, size.Y, 0.0f, Zero3, 1.0f, // top right
            border, size.Y - border, 0.0f, Zero3, Zero6 // bottom right
        ];
        float[] topCenterBorderVertices =
        [
            border, size.Y - border, 0.0f, Zero3, Zero6, // bottom left
            border, size.Y, 0.0f, Zero3, 1.0f, // top left
            size.X - border, size.Y, 0.0f, Zero6, 1.0f, // top right
            size.X - border, size.Y - border, 0.0f, Zero6, Zero6 // bottom right
        ];
        float[] topRightBorderVertices =
        [
            size.X - border, size.Y - border, 0.0f, Zero6, Zero6, // bottom left
            size.X - border, size.Y, 0.0f, Zero6, 1.0f, // top left
            size.X, size.Y, 0.0f, 1.0f, 1.0f, // top right
            size.X, size.Y - border, 0.0f, 1.0f, Zero6 // bottom right
        ];
        float[] centerRightBorderVertices =
        [
            size.X - border, border, 0.0f, Zero6, Zero3, // bottom left
            size.X - border, size.Y - border, 0.0f, Zero6, Zero6, // top left
            size.X, size.Y - border, 0.0f, 1.0f, Zero6, // top right
            size.X, border, 0.0f, 1.0f, Zero3 // bottom right
        ];
        float[] bottomRightBorderVertices =
        [
            size.X - border, 0.0f, 0.0f, Zero6, 0.0f, // bottom left
            size.X - border, border, 0.0f, Zero6, Zero3, // top left
            size.X, border, 0.0f, 1.0f, Zero3, // top right
            size.X, 0.0f, 0.0f, 1.0f, 0.0f // bottom right
        ];
        float[] bottomCenterBorderVertices =
        [
            border, 0.0f, 0.0f, Zero3, 0.0f, // bottom left
            border, border, 0.0f, Zero3, Zero3, // top left
            size.X - border, border, 0.0f, Zero6, Zero3, // top right
            size.X - border, 0.0f, 0.0f, Zero6, 0.0f // bottom right
        ];

        indices =
        [
            0, 3, 2, 0, 2, 1,
            4, 7, 6, 4, 6, 5,
            8, 11, 10, 8, 10, 9,
            12, 15, 14, 12, 14, 13,
            16, 19, 18, 16, 18, 17,
            20, 23, 22, 20, 22, 21,
            24, 27, 26, 24, 26, 25,
            28, 31, 30, 28, 30, 29,
            32, 35, 34, 32, 34, 33,
        ];

        vertices =
        [
            ..contentVertices,
            ..bottomLeftBorderVertices,
            ..centerLeftBorderVertices,
            ..topLeftBorderVertices,
            ..topCenterBorderVertices,
            ..topRightBorderVertices,
            ..centerRightBorderVertices,
            ..bottomRightBorderVertices,
            ..bottomCenterBorderVertices
        ];
    }

    public static Vector2i ProjectToScreen(Vector3 coords, Camera camera)
    {
        Matrix4 viewProjectionMatrix = camera.ViewMatrix * camera.ProjectionMatrix;
        Vector2i windowSize = Lind.Engine.Window?.ClientSize ?? Vector2i.Zero;
        Vector3 project = Vector3.Project(coords,
            0, 0, windowSize.X, windowSize.Y, camera.NearClip, camera.FarClip,
            viewProjectionMatrix);

        return new Vector2i((int)project.X, (int)project.Y);
    }

    public static Vector3 UnProjectToWorld(Vector2 coords, Camera camera)
    {
        Matrix4 viewProjectionMatrix = camera.ViewMatrix * camera.ProjectionMatrix;
        Vector2i windowSize = Lind.Engine.Window?.ClientSize ?? Vector2i.Zero;
        return Vector3.Unproject(new Vector3(coords),
            0, 0, windowSize.X, windowSize.Y, camera.NearClip, camera.FarClip,
            Matrix4.Invert(viewProjectionMatrix));
    }
}