using System;

namespace LindEngine.Core.Exceptions;

public class GuiElementNotExistsException : Exception
{
    public GuiElementNotExistsException(string message) : base(message)
    {
    }
}