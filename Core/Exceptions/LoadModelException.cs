using System;

namespace LindEngine.Core.Exceptions;

public class LoadModelException : Exception
{
    public LoadModelException(string message) : base(message)
    {
    }
}