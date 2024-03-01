using OpenTK.Graphics.OpenGL4;

namespace Lindengine.Utilities.BufferObject;

internal class ElementBuffer
{
    internal readonly int Name;
    private uint[] _indices = [];
    
    public ElementBuffer()
    {
        GL.CreateBuffers(1, out Name);
    }
    
    public void SetIndices(uint[] indices)
    {
        if (indices.Length != _indices.Length)
        {
            GL.NamedBufferData(Name, indices.Length * sizeof(float), indices, BufferUsageHint.StaticDraw);
        }
        else
        {
            GL.NamedBufferSubData(Name, 0, indices.Length * sizeof(float), indices);
        }
        
        _indices = indices;
    }

    public void Unload()
    {
        GL.DeleteBuffer(Name);
    }
}