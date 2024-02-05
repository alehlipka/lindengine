using lindengine.common.textures;
using lindengine.gui.font;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace lindengine.gui
{
    public class Text(string name, Vector2i size, string text, int fontSize, Color4 textColor) : Element(name, size)
    {
        protected string text = text;
        protected int fontSize = fontSize;
        protected Color4 textColor = textColor;
        protected byte[] fontBitmapBytes = [];
        protected Texture? texture;

        public void SetText(string newText)
        {
            if (!text.Equals(newText))
            {
                text = newText;
                fontBitmapBytes = FontManager.GetBitmapBytes("droidsans", size, text, fontSize, textColor);
                texture = Texture.LoadFromBytes($"{Name}_texture", fontBitmapBytes, size);
            }
        }

        protected override void OnLoad(Element element, Vector2i windowSize)
        {
            fontBitmapBytes = FontManager.GetBitmapBytes("droidsans", size, text, fontSize, textColor);
            texture = Texture.LoadFromBytes($"{Name}_texture", fontBitmapBytes, size);
            modelMatrix = Matrix4.CreateTranslation(new Vector3(10, windowSize.Y - size.Y - 10, 0));
        }

        protected override void OnContextResize(Element element, ResizeEventArgs args)
        {
            modelMatrix = Matrix4.CreateTranslation(new Vector3(10, args.Height - size.Y - 10, 0));
        }

        protected override void OnRenderFrame(Element element, FrameEventArgs args)
        {
            texture?.Use();
        }
    }
}
