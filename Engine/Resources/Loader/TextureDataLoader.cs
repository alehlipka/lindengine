using Lindengine.Graphics;
using OpenTK.Mathematics;
using StbImageSharp;

namespace Lindengine.Resources.Loader;

internal static class TextureDataLoader
{
    internal static TextureData LoadResource(string path)
    {
        byte[] data = File.ReadAllBytes(path);
        ImageResult image = ImageResult.FromMemory(data, ColorComponents.RedGreenBlueAlpha);

        return new TextureData(image.Data, new Vector2i(image.Width, image.Height));
    }
}
