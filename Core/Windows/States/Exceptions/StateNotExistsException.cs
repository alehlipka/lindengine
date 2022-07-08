using System;

namespace LindEngine.Core.Windows.States.Exceptions;

public class StateNotExistsException : Exception
{
    public StateNotExistsException(string message) : base(message)
    {
    }
}