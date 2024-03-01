using OpenTK.Graphics.OpenGL4;

namespace Lindengine.Utilities.BufferObject;

internal class VertexBuffer
{
    internal readonly int Name;
    private float[] _vertices = [];

    public VertexBuffer()
    {
        GL.CreateBuffers(1, out Name);
    }

    public void SetVertices(float[] vertices)
    {
        if (vertices.Length != _vertices.Length)
        {
            GL.NamedBufferData(Name, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
        }
        else
        {
            GL.NamedBufferSubData(Name, 0, vertices.Length * sizeof(float), vertices);
        }
        
        _vertices = vertices;
    }

    public void Unload()
    {
        GL.DeleteBuffer(Name);
    }
}