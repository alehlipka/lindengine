namespace lindengine.gui.font;

internal abstract class BaseFontHandler : IFontHandler
{
    private IFontHandler? _nextHandler;

    public IFontHandler SetNext(IFontHandler handler)
    {
        _nextHandler = handler;

        return handler;
    }

    public string Handle(string characters)
    {
        string processedCharacters = Process(characters);

        return _nextHandler?.Handle(processedCharacters) ?? processedCharacters;
    }

    protected abstract string Process(string characters);
}
