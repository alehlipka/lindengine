namespace lindengine.gui.font;

internal class FirstHandler : BaseFontHandler
{
    protected override string Process(string characters)
    {
        Console.WriteLine("First handler");

        return characters + "First handler";
    }
}
