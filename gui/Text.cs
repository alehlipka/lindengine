using lindengine.common.textures;
using lindengine.gui.font;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace lindengine.gui
{
    public class Text(string name, string text, int fontSize, FontIncrease increase, Color4 textColor) : Element(name)
    {
        protected string text = text;
        protected int fontSize = fontSize;
        protected FontIncrease increase = increase;
        protected Color4 textColor = textColor;
        protected byte[] fontBitmapBytes = [];

        protected override void OnLoad(Element element, Vector2i windowSize)
        {
            Size = new Vector2i(800, fontSize * 3);
            fontBitmapBytes = FontManager.GetBitmapBytes("droidsans", Size, text, fontSize, textColor);
            LoadTexture(Texture.LoadFromBytes($"{Name}_texture", fontBitmapBytes, Size));

            modelMatrix = Matrix4.CreateTranslation(new Vector3(10, windowSize.Y - Size.Y - 10, 0));
        }

        public void SetText(string newText)
        {
            if (!text.Equals(newText))
            {
                text = newText;
                fontBitmapBytes = FontManager.GetBitmapBytes("droidsans", Size, text, fontSize, textColor);
                LoadTexture(Texture.LoadFromBytes($"{Name}_texture", fontBitmapBytes, Size));
            }
        }

        protected override void LoadTexture(Texture texture)
        {
            textures.Clear();
            textures.Add(texture);
        }

        protected override void OnContextResize(Element element, ResizeEventArgs args)
        {
            modelMatrix = Matrix4.CreateTranslation(new Vector3(10, args.Height - Size.Y - 10, 0));
        }
    }
}
