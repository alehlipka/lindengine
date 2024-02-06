namespace lindengine.gui.font;

internal class SecondHandler : BaseFontHandler
{
    protected override string Process(string characters)
    {
        Console.WriteLine("Second handler");

        return characters + "Second handler";
    }
}
