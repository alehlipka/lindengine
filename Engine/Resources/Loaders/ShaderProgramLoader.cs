using Lindengine.Graphics.Shader;
using OpenTK.Graphics.OpenGL4;

namespace Lindengine.Resources.Loaders;

internal static class ShaderProgramLoader
{
    internal static ShaderProgram LoadResource(string path)
    {
        string vertexShaderCode = File.ReadAllText(Path.Combine(path, "vertex.glsl"));
        string fragmentShaderCode = File.ReadAllText(Path.Combine(path, "fragment.glsl"));
        
        int vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, vertexShaderCode);
        GL.CompileShader(vertexShader);
        GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out int vertexCompileCode);
        if (vertexCompileCode != 1)
        {
            string vertexInfoLog = GL.GetShaderInfoLog(vertexShader);
            throw new Exception($"Shader ({vertexShader}) compiling error.\n\n{vertexInfoLog}");
        }

        int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, fragmentShaderCode);
        GL.CompileShader(fragmentShader);
        GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out int fragmentCompileCode);
        if (fragmentCompileCode != 1)
        {
            string fragmentInfoLog = GL.GetShaderInfoLog(fragmentShader);
            throw new Exception($"Shader ({fragmentShader}) compiling error.\n\n{fragmentInfoLog}");
        }

        int handle = GL.CreateProgram();
        GL.AttachShader(handle, vertexShader);
        GL.AttachShader(handle, fragmentShader);
        GL.LinkProgram(handle);
        GL.GetProgram(handle, GetProgramParameterName.LinkStatus, out int linkCode);
        if (linkCode != 1)
        {
            string linkInfoLog = GL.GetProgramInfoLog(handle);
            throw new Exception($"Program ({handle}) linking error.\n\n{linkInfoLog}");
        }

        GL.DetachShader(handle, vertexShader);
        GL.DetachShader(handle, fragmentShader);
        GL.DeleteShader(fragmentShader);
        GL.DeleteShader(vertexShader);

        return new ShaderProgram(handle);
    }
}
