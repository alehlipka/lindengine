using lindengine.common.textures;
using lindengine.gui.font;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace lindengine.gui
{
    public class Text(string name, string text, int fontSize, Color4 textColor) : Element(name)
    {
        protected string text = text;
        protected int fontSize = fontSize;
        protected Color4 textColor = textColor;

        protected override void OnLoad(Element element, Vector2i windowSize)
        {
            Size = new Vector2i(800, fontSize * 7);
            LoadTexture($"{Name}_texture", new TextureData(FontManager.GetBitmapBytes("opensansbold", Size, text, fontSize, textColor), Size));

            modelMatrix = Matrix4.CreateTranslation(new Vector3(10, windowSize.Y - Size.Y - 10, 0));
        }

        public void SetText(string newText)
        {
            if (!IsLoaded) return;

            if (!text.Equals(newText))
            {
                text = newText;
                ChangeTexture($"{Name}_texture", new TextureData(FontManager.GetBitmapBytes("opensansbold", Size, text, fontSize, textColor), Size));
            }
        }

        protected override void OnContextResize(Element element, ResizeEventArgs args)
        {
            modelMatrix = Matrix4.CreateTranslation(new Vector3(10, args.Height - Size.Y - 10, 0));
        }
    }
}
