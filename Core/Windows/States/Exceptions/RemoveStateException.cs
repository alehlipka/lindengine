using System;

namespace LindEngine.Core.Windows.States.Exceptions;

public class RemoveStateException : Exception
{
    public RemoveStateException(string message) : base(message)
    {
    }
}