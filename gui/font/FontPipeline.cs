namespace lindengine.gui.font;

internal class FontPipeline
{
    private readonly FirstHandler _firstHandler;

    public FontPipeline()
    {
        _firstHandler = new FirstHandler();

        _firstHandler
            .SetNext(new SecondHandler())
            .SetNext(new SecondHandler());
    }

    public string Process(string characters)
    {
        return _firstHandler.Handle(characters);
    }
}
