using OpenTK.Graphics.OpenGL4;

namespace Lindengine.Utilities.BufferObject;

internal class VertexBuffer
{
    internal readonly int Name;
    internal float[] Vertices = [];

    internal VertexBuffer()
    {
        GL.CreateBuffers(1, out Name);
    }

    internal void SetVertices(float[] vertices)
    {
        if (vertices.Length != Vertices.Length)
        {
            GL.NamedBufferData(Name, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
        }
        else
        {
            GL.NamedBufferSubData(Name, 0, vertices.Length * sizeof(float), vertices);
        }
        
        Vertices = vertices;
    }

    internal void Unload()
    {
        GL.DeleteBuffer(Name);
    }
}