using System.Collections;

namespace lindengine.gui.font;

internal class CharactersCollection(FontIncrease fontIncrease) : FontIteratorAggregate
{
    private readonly FontIncrease _fontIncrease = fontIncrease;

    public override IEnumerator GetEnumerator()
    {
        switch (_fontIncrease)
        {
            case FontIncrease.Horizontal:
                return new InfinityWidthIterator(this);
            case FontIncrease.Vertical:
                return new InfinityHeightIterator(this);
        }

        throw new NotImplementedException();
    }
}
