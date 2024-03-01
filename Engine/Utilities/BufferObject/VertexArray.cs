using Lindengine.Graphics.Shader;
using OpenTK.Graphics.OpenGL4;

namespace Lindengine.Utilities.BufferObject;

internal class VertexArray
{
    private readonly int _name;

    internal VertexArray()
    {
        GL.CreateVertexArrays(1, out _name);
    }

    internal void LinkAttributes(int vertexBufferName, int elementBufferName, ShaderProgram shader)
    {
        int positionAttribute = shader.GetAttribLocation("aPosition");
        int textureAttribute = shader.GetAttribLocation("aTexture");
        
        GL.EnableVertexArrayAttrib(_name, positionAttribute);
        GL.VertexArrayAttribBinding(_name, positionAttribute, 0);
        GL.VertexArrayAttribFormat(_name, positionAttribute, 3, VertexAttribType.Float, false, 0);
        
        GL.EnableVertexArrayAttrib(_name, textureAttribute);
        GL.VertexArrayAttribBinding(_name, textureAttribute, 0);
        GL.VertexArrayAttribFormat(_name, textureAttribute, 2, VertexAttribType.Float, false, 3 * sizeof(float));
        
        GL.VertexArrayVertexBuffer(_name, 0, vertexBufferName, 0, 5 * sizeof(float));
        GL.VertexArrayElementBuffer(_name, elementBufferName);
    }

    internal void Use()
    {
        GL.BindVertexArray(_name);
    }

    internal void Unload()
    {
        GL.DeleteVertexArray(_name);
    }
}