using lindengine.common.textures;
using lindengine.gui.font;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace lindengine.gui
{
    public enum FontIncrease
    {
        Horizontal,
        Vertical,
        Both
    }

    public class Text(string name, string text, int fontSize, FontIncrease increase, Color4 textColor) : Element(name)
    {
        protected string text = text;
        protected int fontSize = fontSize;
        protected FontIncrease increase = increase;
        protected Color4 textColor = textColor;
        protected byte[] fontBitmapBytes = [];

        protected override void OnLoad(Element element, Vector2i windowSize)
        {
            // fontBitmapBytes = FontManager.GetBitmapBytes("droidsans", size, text, fontSize, textColor);
            // LoadTexture(Texture.LoadFromBytes($"{Name}_texture", fontBitmapBytes, size));

            modelMatrix = Matrix4.CreateTranslation(new Vector3(10, windowSize.Y - Size.Y - 100, 0));
        }

        public void SetText(string newText)
        {
            if (!text.Equals(newText))
            {
                text = newText;
                // fontBitmapBytes = FontManager.GetBitmapBytes("droidsans", size, text, fontSize, textColor);
                // LoadTexture(Texture.LoadFromBytes($"{Name}_texture", fontBitmapBytes, size));
            }
        }

        protected override void OnContextResize(Element element, ResizeEventArgs args)
        {
            modelMatrix = Matrix4.CreateTranslation(new Vector3(10, args.Height - Size.Y - 100, 0));
        }
    }
}
