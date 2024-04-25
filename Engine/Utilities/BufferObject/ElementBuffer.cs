using OpenTK.Graphics.OpenGL4;

namespace Lindengine.Utilities.BufferObject;

internal class ElementBuffer
{
    internal readonly int Name;
    internal uint[] Indices = [];
    
    internal ElementBuffer()
    {
        GL.CreateBuffers(1, out Name);
    }
    
    internal void SetIndices(uint[] indices)
    {
        if (indices.Length != Indices.Length)
        {
            GL.NamedBufferData(Name, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
        }
        else
        {
            GL.NamedBufferSubData(Name, 0, indices.Length * sizeof(uint), indices);
        }
        
        Indices = indices;
    }

    internal void Unload()
    {
        GL.DeleteBuffer(Name);
    }
}
