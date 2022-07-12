using System;

namespace LindEngine.Core.Exceptions;

public class ShaderNotExistsException : Exception
{
    public ShaderNotExistsException(string message) : base(message)
    {
    }
}