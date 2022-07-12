using System;

namespace LindEngine.Core.Exceptions;

public class StateNotExistsException : Exception
{
    public StateNotExistsException(string message) : base(message)
    {
    }
}