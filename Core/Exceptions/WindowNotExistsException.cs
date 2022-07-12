using System;

namespace LindEngine.Core.Exceptions;

public class WindowNotExistsException : Exception
{
    public WindowNotExistsException(string message) : base(message)
    {
    }
}