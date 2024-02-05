using lindengine.common.textures;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace lindengine.gui;

public class Background(string name, Vector2i size, string path) : Element(name, size)
{
    protected string texturePath = path;

    protected override void OnLoad(Element element, Vector2i windowSize)
    {
        size = windowSize;
        LoadTexture(Texture.LoadFromFile($"{Name}_texture", texturePath));
    }

    protected override void OnContextResize(Element element, ResizeEventArgs args)
    {
        size = args.Size;
        
        InitializeVertices();
        BindVertexBuffer();
    }
}
