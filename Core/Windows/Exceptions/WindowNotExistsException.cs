using System;

namespace LindEngine.Core.Windows.Exceptions;

public class WindowNotExistsException : Exception
{
    public WindowNotExistsException(string message) : base(message)
    {
    }
}