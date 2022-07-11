using System;

namespace LindEngine.Core.Shaders.Exceptions;

public class ShaderNotExistsException : Exception
{
    public ShaderNotExistsException(string message) : base(message)
    {
    }
}