using Lindengine.Core;
using Lindengine.Graphics;
using Lindengine.Graphics.Shader;
using Lindengine.Utilities.BufferObject;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Lindengine.Utilities.Bounding;

public class BoundingBox2d
{
    private readonly Texture _boundingTexture = Lind.Engine.Resources.Load<Texture>(Path.Combine("Assets", "green.jpg"));
    private readonly BuffersContainer _buffersContainer = new();
    private Vector3 _minVector;
    private Vector3 _maxVector;
    private Matrix4 _modelMatrix = Matrix4.Identity;

    public void Calculate(Matrix4 modelMatrix, Vector2i size)
    {
        if (modelMatrix == _modelMatrix) return;
        
        _modelMatrix = modelMatrix;
        
        Vector3 lbOriginal = Vector3.Zero;
        Vector3 ltOriginal = new(0.0f, size.Y, 0.0f);
        Vector3 rbOriginal = new(size.X, 0.0f, 0.0f);
        Vector3 rtOriginal = new(size.X, size.Y, 0.0f);

        Vector3[] vectors = [
            Vector3.TransformVector(lbOriginal, _modelMatrix),
            Vector3.TransformVector(ltOriginal, _modelMatrix),
            Vector3.TransformVector(rbOriginal, _modelMatrix),
            Vector3.TransformVector(rtOriginal, _modelMatrix),
        ];

        _minVector = Vector3.PositiveInfinity;
        _maxVector = Vector3.NegativeInfinity;
        foreach (Vector3 vector in vectors)
        {
            _minVector = Vector3.ComponentMin(vector, _minVector);
            _maxVector = Vector3.ComponentMax(vector, _maxVector);
        }
        
        UtilityFunctions.GetBoundingVertices(_minVector, _maxVector, out uint[] indices, out float[] vertices);
        indices = [0,3,2,1];
        _buffersContainer.SetIndices(indices);
        _buffersContainer.SetVertices(vertices);
    }

    public void Render(ShaderProgram shader)
    {
        shader.SetUniformData("modelMatrix", _modelMatrix.ClearScale().ClearRotation());
        _boundingTexture.Use();
        _buffersContainer.LinkShaderAttributes(shader);
        _buffersContainer.Draw(PrimitiveType.LineLoop);
    }

    public void Unload()
    {
        _boundingTexture.Unload();
        _buffersContainer.Unload();
    }
}