using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace LindEngine.Core.Shaders;

public class LindenShader
{
    public readonly string Name;

    private readonly int _handle;
    private readonly Dictionary<string, int> _uniformLocations;

    public LindenShader(string path)
    {
        string shaderDirectory = path;
        Name = Path.GetFileName(shaderDirectory);

        string vertexSource = LoadSource(Path.Combine(shaderDirectory, "vertex.glsl"));
        int vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, vertexSource);
        CompileShader(vertexShader);
        
        string fragmentSource = LoadSource(Path.Combine(shaderDirectory, "fragment.glsl"));
        int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, fragmentSource);
        CompileShader(fragmentShader);

        _handle = GL.CreateProgram();
        GL.AttachShader(_handle, vertexShader);
        GL.AttachShader(_handle, fragmentShader);
        
        LinkProgram(_handle);
        
        GL.DetachShader(_handle, vertexShader);
        GL.DetachShader(_handle, fragmentShader);
        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);
        
        GL.GetProgram(_handle, GetProgramParameterName.ActiveUniforms, out int numberOfUniforms);
        _uniformLocations = new Dictionary<string, int>();
        for (int i = 0; i < numberOfUniforms; i++)
        {
            string key = GL.GetActiveUniform(_handle, i, out _, out _);
            int location = GL.GetUniformLocation(_handle, key);
            _uniformLocations.Add(key, location);
        }
    }
    
    public void Use()
    {
        GL.UseProgram(_handle);
    }
    
    public int GetAttribLocation(string attribName)
    {
        return GL.GetAttribLocation(_handle, attribName);
    }

    public void Dispose()
    {
        GL.DeleteProgram(_handle);
    }
    
    public void SetInt(string name, int data)
    {
        GL.UseProgram(_handle);
        GL.Uniform1(_uniformLocations[name], data);
    }

    public void SetFloat(string name, float data)
    {
        GL.UseProgram(_handle);
        GL.Uniform1(_uniformLocations[name], data);
    }

    public void SetMatrix4(string name, Matrix4 data)
    {
        GL.UseProgram(_handle);
        GL.UniformMatrix4(_uniformLocations[name], true, ref data);
    }

    public void SetMatrix4Raw(string name, Matrix4 data)
    {
        GL.UseProgram(_handle);
        int location = GL.GetUniformLocation(_handle, name);
        GL.UniformMatrix4(location, false, ref data);
    }

    public void SetVector3(string name, Vector3 data)
    {
        GL.UseProgram(_handle);
        GL.Uniform3(_uniformLocations[name], data);
    }

    public void SetVector4(string name, Vector4 data)
    {
        GL.UseProgram(_handle);
        GL.Uniform4(_uniformLocations[name], data);
    }

    private string LoadSource(string path)
    {
        using StreamReader reader = new StreamReader(path, Encoding.UTF8);
        return reader.ReadToEnd();
    }

    private void CompileShader(int shader)
    {
        GL.CompileShader(shader);
        GL.GetShader(shader, ShaderParameter.CompileStatus, out int code);
        if (code == (int)All.True) return;
        string infoLog = GL.GetShaderInfoLog(shader);
        throw new Exception($"Error occurred whilst compiling Shader({shader}).\n\n{infoLog}");
    }

    private void LinkProgram(int program)
    {
        GL.LinkProgram(program);
        GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int code);
        if (code == (int)All.True) return;
        throw new Exception(GL.GetProgramInfoLog(program));
    }
}