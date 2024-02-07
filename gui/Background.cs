using lindengine.common.textures;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace lindengine.gui;

public class Background(string name, string path) : Element(name)
{
    protected string texturePath = path;

    protected override void OnLoad(Element element, Vector2i windowSize)
    {
        Size = windowSize;
        LoadFileTexture($"{Name}_texture", texturePath);
    }

    protected override void OnContextResize(Element element, ResizeEventArgs args)
    {
        Size = args.Size;

        InitializeVertices();
        ResetVertexBuffer();
    }
}
