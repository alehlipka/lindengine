using Lindengine.Graphics.Font;

namespace Lindengine.Resources.Loaders;

public static class FontLoader
{
    internal static Font LoadResource(string path)
    {
        byte[] data = File.ReadAllBytes(path);

        return new Font(data);
    }
}
