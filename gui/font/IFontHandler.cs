namespace lindengine.gui.font;

internal interface IFontHandler
{
    string Handle(string characters);
    IFontHandler SetNext(IFontHandler handler);
}
