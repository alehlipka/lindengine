using System.Collections;

namespace lindengine.gui.font;

internal abstract class FontIteratorAggregate : IEnumerable
{
    public abstract IEnumerator GetEnumerator();
}
