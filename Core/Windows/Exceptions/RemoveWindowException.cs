using System;

namespace LindEngine.Core.Windows.Exceptions;

public class RemoveWindowException : Exception
{
    public RemoveWindowException(string message) : base(message)
    {
    }
}