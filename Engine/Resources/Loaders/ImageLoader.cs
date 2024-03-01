using OpenTK.Windowing.Common.Input;
using StbImageSharp;

namespace Lindengine.Resources.Loaders;

internal static class ImageLoader
{
    internal static Image LoadResource(string path)
    {
        byte[] data = File.ReadAllBytes(path);
        ImageResult image = ImageResult.FromMemory(data, ColorComponents.RedGreenBlueAlpha);

        return new Image(image.Width, image.Height, image.Data);
    }
}