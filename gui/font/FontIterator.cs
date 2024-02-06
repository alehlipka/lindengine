using System.Collections;

namespace lindengine.gui.font;

internal abstract class FontIterator : IEnumerator
{
    object IEnumerator.Current => Current();

    public abstract int Key();
    public abstract object Current();
    public abstract bool MoveNext();
    public abstract void Reset();
}
