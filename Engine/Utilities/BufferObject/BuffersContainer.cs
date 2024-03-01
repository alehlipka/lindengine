using Lindengine.Graphics.Shader;

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
    
    public void Use()
    {
        _vertexArray.Use();
    }

    public void Unload()
    {
        _elementBuffer.Unload();
        _vertexBuffer.Unload();
        _vertexArray.Unload();
    }
}