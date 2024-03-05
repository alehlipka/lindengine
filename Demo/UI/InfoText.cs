using Lindengine.Core;
using Lindengine.Graphics;
using Lindengine.Graphics.Font;
using Lindengine.Graphics.Shader;
using Lindengine.Output.Camera;
using Lindengine.UI;
using Lindengine.Utilities;
using OpenTK.Mathematics;

namespace Demo.UI;

public class InfoText(Vector2i size, ShaderProgram shader) : UiElement(size, shader)
{
    private readonly DebugInformation _debugInformation = new();
    private readonly TextBuilder _textBuilder = new();
    private const int FontSize = 22;
    private readonly Font _font = Lind.Engine.Resources.Load<Font>(Path.Combine("Assets", "Fonts", "OpenSansBold.ttf"));
    private string _debugText = string.Empty;

    protected override void OnLoad()
    {
        Texture = new Texture(_textBuilder.Draw(_debugText, _font, FontSize, Color4.White, Size, TextAlign.Left));
    }

    protected override void OnWindowResize(Vector2i size)
    {
        Size = size;
        Texture = new Texture(_textBuilder.Draw(_debugText, _font, FontSize, Color4.White, Size, TextAlign.Left));
    }

    protected override void OnRender(Camera camera, double elapsedSeconds)
    {
        _debugText = _debugInformation.CalculateDraw(elapsedSeconds);
        if (_debugInformation.IsChanged())
        {
            Texture?.UpdateData(_textBuilder.Draw(_debugText, _font, FontSize, Color4.White, Size, TextAlign.Left));
        }
    }
}