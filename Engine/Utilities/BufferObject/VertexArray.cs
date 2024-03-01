using Lindengine.Graphics.Shader;
using OpenTK.Graphics.OpenGL4;

namespace Lindengine.Utilities.BufferObject;

internal class VertexArray
{
    private readonly int _name;
    private VertexBuffer? _vertexBuffer;
    private ElementBuffer? _elementBuffer;

    public VertexArray()
    {
        GL.CreateVertexArrays(1, out _name);
    }

    public void SetBuffers(VertexBuffer vertexBuffer, ElementBuffer elementBuffer, ShaderProgram shader)
    {
        _vertexBuffer = vertexBuffer;
        _elementBuffer = elementBuffer;
        
        int positionAttribute = shader.GetAttribLocation("aPosition");
        int textureAttribute = shader.GetAttribLocation("aTexture");
        
        GL.EnableVertexArrayAttrib(_name, positionAttribute);
        GL.VertexArrayAttribBinding(_name, positionAttribute, 0);
        GL.VertexArrayAttribFormat(_name, positionAttribute, 3, VertexAttribType.Float, false, 0);
        
        GL.EnableVertexArrayAttrib(_name, textureAttribute);
        GL.VertexArrayAttribBinding(_name, textureAttribute, 0);
        GL.VertexArrayAttribFormat(_name, textureAttribute, 2, VertexAttribType.Float, false, 3 * sizeof(float));
        
        GL.VertexArrayVertexBuffer(_name, 0, _vertexBuffer.Name, 0, 5 * sizeof(float));
        GL.VertexArrayElementBuffer(_name, _elementBuffer.Name);
    }

    public void Use()
    {
        GL.BindVertexArray(_name);
    }

    public void Unload()
    {
        GL.DeleteVertexArray(_name);
    }
}