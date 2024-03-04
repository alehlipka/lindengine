using Lindengine.Core;
using Lindengine.Output.Camera;
using OpenTK.Mathematics;

namespace Lindengine.Utilities;

internal static class UtilityFunctions
{
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

    public static void GetBorderedVertices(Vector2i size, Vector4 border, out uint[] indices, out float[] vertices)
    {
        const float zero3 = 1.0f / 3.0f;
        const float zero6 = zero3 * 2.0f;

        float borderTop = border.X;
        float borderRight = border.Y;
        float borderBottom = border.Z;
        float borderLeft = border.W;

        if (borderLeft == 0) borderLeft = size.X / 3.0f;
        if (borderRight == 0) borderRight = size.X / 3.0f;
        if (borderTop == 0) borderTop = size.Y / 3.0f;
        if (borderBottom == 0) borderBottom = size.Y / 3.0f;
        
        float[] contentVertices =
        [
            borderLeft, borderBottom, 0.0f, zero3, zero3, // bottom left
            borderLeft, size.Y - borderTop, 0.0f, zero3, zero6, // top left
            size.X - borderRight, size.Y - borderTop, 0.0f, zero6, zero6, // top right
            size.X - borderRight, borderBottom, 0.0f, zero6, zero3 // bottom right
        ];
        
        float[] bottomLeftBorderVertices =
        [
            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // bottom left
            0.0f, borderBottom, 0.0f, 0.0f, zero3, // top left
            borderLeft, borderBottom, 0.0f, zero3, zero3, // top right
            borderLeft, 0.0f, 0.0f, zero3, 0.0f // bottom right
        ];
        float[] centerLeftBorderVertices =
        [
            0.0f, borderBottom, 0.0f, 0.0f, zero3, // bottom left
            0.0f, size.Y - borderTop, 0.0f, 0.0f, zero6, // top left
            borderLeft, size.Y - borderTop, 0.0f, zero3, zero6, // top right
            borderLeft, borderBottom, 0.0f, zero3, zero3 // bottom right
        ];
        float[] topLeftBorderVertices =
        [
            0.0f, size.Y - borderTop, 0.0f, 0.0f, zero6, // bottom left
            0.0f, size.Y, 0.0f, 0.0f, 1.0f, // top left
            borderLeft, size.Y, 0.0f, zero3, 1.0f, // top right
            borderLeft, size.Y - borderTop, 0.0f, zero3, zero6 // bottom right
        ];
        float[] topCenterBorderVertices =
        [
            borderLeft, size.Y - borderTop, 0.0f, zero3, zero6, // bottom left
            borderLeft, size.Y, 0.0f, zero3, 1.0f, // top left
            size.X - borderRight, size.Y, 0.0f, zero6, 1.0f, // top right
            size.X - borderRight, size.Y - borderTop, 0.0f, zero6, zero6 // bottom right
        ];
        float[] topRightBorderVertices =
        [
            size.X - borderRight, size.Y - borderTop, 0.0f, zero6, zero6, // bottom left
            size.X - borderRight, size.Y, 0.0f, zero6, 1.0f, // top left
            size.X, size.Y, 0.0f, 1.0f, 1.0f, // top right
            size.X, size.Y - borderTop, 0.0f, 1.0f, zero6 // bottom right
        ];
        float[] centerRightBorderVertices =
        [
            size.X - borderRight, borderBottom, 0.0f, zero6, zero3, // bottom left
            size.X - borderRight, size.Y - borderTop, 0.0f, zero6, zero6, // top left
            size.X, size.Y - borderTop, 0.0f, 1.0f, zero6, // top right
            size.X, borderBottom, 0.0f, 1.0f, zero3 // bottom right
        ];
        float[] bottomRightBorderVertices =
        [
            size.X - borderRight, 0.0f, 0.0f, zero6, 0.0f, // bottom left
            size.X - borderRight, borderBottom, 0.0f, zero6, zero3, // top left
            size.X, borderBottom, 0.0f, 1.0f, zero3, // top right
            size.X, 0.0f, 0.0f, 1.0f, 0.0f // bottom right
        ];
        float[] bottomCenterBorderVertices =
        [
            borderLeft, 0.0f, 0.0f, zero3, 0.0f, // bottom left
            borderLeft, borderBottom, 0.0f, zero3, zero3, // top left
            size.X - borderRight, borderBottom, 0.0f, zero6, zero3, // top right
            size.X - borderRight, 0.0f, 0.0f, zero6, 0.0f // bottom right
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