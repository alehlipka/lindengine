using System.Collections.Generic;

namespace LindEngine.Core.Managers;

public abstract class LindenManager
{
    protected abstract void AddItems();
    public abstract List<string> GetNames();
    public abstract void Select(string name);
}