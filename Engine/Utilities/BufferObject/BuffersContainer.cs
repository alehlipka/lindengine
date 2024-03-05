using Lindengine.Graphics.Shader;
using OpenTK.Graphics.OpenGL4;

namespace Lindengine.Utilities.BufferObject;

public class BuffersContainer
{
    private readonly VertexArray _vertexArray = new();
    private readonly VertexBuffer _vertexBuffer = new();
    private readonly ElementBuffer _elementBuffer = new();

    public void SetVertices(float[] vertices)
    {
        _vertexBuffer.SetVertices(vertices);
    }

    public void SetIndices(uint[] indices)
    {
        _elementBuffer.SetIndices(indices);
    }

    public void LinkShaderAttributes(ShaderProgram shader)
    {
        _vertexArray.LinkAttributes(_vertexBuffer.Name, _elementBuffer.Name, shader);
    }
    
    public void Draw(PrimitiveType primitiveType = PrimitiveType.Triangles)
    {
        _vertexArray.Use();
        GL.DrawElements(primitiveType, _elementBuffer.Indices.Length, DrawElementsType.UnsignedInt, 0);
    }

    public void Unload()
    {
        _elementBuffer.Unload();
        _vertexBuffer.Unload();
        _vertexArray.Unload();
    }
}