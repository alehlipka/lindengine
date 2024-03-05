using Lindengine.UI;
using OpenTK.Mathematics;

namespace Lindengine.Utilities;

public sealed class ModelMatrix
{
    public ElementOrigin Origin { get; private set; }
    public Vector3 Position { get; private set; }
    public Vector3 Angle { get; private set; }
    public Vector3 Scale { get; private set; }

    private Matrix4 _originMatrix = Matrix4.Identity;
    private Matrix4 _translationMatrix = Matrix4.Identity;
    private Matrix4 _rotationMatrix = Matrix4.Identity;
    private Matrix4 _scaleMatrix = Matrix4.Identity;
    private Matrix4 _parentMatrix = Matrix4.Identity;
    private Matrix4 _modelMatrix = Matrix4.Identity;

    private bool _isModelMatrixDirty;
    
    internal event VoidDelegate? ModelMatrixChanged;

    public void SetOrigin(ElementOrigin origin, Vector2i size)
    {
        if (Origin == origin) return;
        _isModelMatrixDirty = true;
        
        Origin = origin;
        _originMatrix = Origin switch
        {
            ElementOrigin.BottomLeft => Matrix4.CreateTranslation(0, 0, 0),
            ElementOrigin.BottomRight => Matrix4.CreateTranslation(-size.X, 0, 0),
            ElementOrigin.TopLeft => Matrix4.CreateTranslation(0, -size.Y, 0),
            ElementOrigin.TopRight => Matrix4.CreateTranslation(-size.X, -size.Y, 0),
            ElementOrigin.Center => Matrix4.CreateTranslation(-size.X / 2.0f, -size.Y / 2.0f, 0),
            _ => Matrix4.Identity
        };
        
        ModelMatrixChanged?.Invoke();
    }
    
    public void SetTranslation(Vector3 position)
    {
        if (Position == position) return;
        
        Position = position;
        _translationMatrix = Matrix4.CreateTranslation(Position);
        _isModelMatrixDirty = true;
        
        ModelMatrixChanged?.Invoke();
    }

    public void SetRotation(Vector3 angle)
    {
        if (Angle == angle) return;
        
        Angle = angle;
        _rotationMatrix = Matrix4.CreateFromQuaternion(new Quaternion(Angle));
        _isModelMatrixDirty = true;
        
        ModelMatrixChanged?.Invoke();
    }

    public void SetScale(Vector3 scale)
    {
        if (Scale == scale) return;
        
        Scale = scale;
        _scaleMatrix = Matrix4.CreateScale(Scale);
        _isModelMatrixDirty = true;
        
        ModelMatrixChanged?.Invoke();
    }

    public void SetParentMatrix(Matrix4 parent)
    {
        if (_parentMatrix == parent) return;
        
        _parentMatrix = parent;
        _isModelMatrixDirty = true;
        
        ModelMatrixChanged?.Invoke();
    }

    public Matrix4 GetMatrix()
    {
        if (!_isModelMatrixDirty) return _modelMatrix;
        
        _modelMatrix = _originMatrix * _scaleMatrix * _rotationMatrix * _translationMatrix * _parentMatrix;
        _isModelMatrixDirty = false;

        return _modelMatrix;
    }
}