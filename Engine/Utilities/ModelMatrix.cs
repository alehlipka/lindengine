using Lindengine.UI;
using OpenTK.Mathematics;

namespace Lindengine.Utilities;

public class ModelMatrix
{
    internal event VoidDelegate? OnChange;
    
    private Matrix4 _originMatrix = Matrix4.Identity;
    private Matrix4 _translationMatrix = Matrix4.Identity;
    private Matrix4 _rotationMatrix = Matrix4.Identity;
    private Matrix4 _scaleMatrix = Matrix4.Identity;
    private Matrix4 _parentMatrix = Matrix4.Identity;
    private Matrix4 _modelMatrix = Matrix4.Identity;

    public void Origin(ElementOrigin origin, Vector2i size)
    {
        _originMatrix = origin switch
        {
            ElementOrigin.BottomLeft => Matrix4.CreateTranslation(0, 0, 0),
            ElementOrigin.BottomRight => Matrix4.CreateTranslation(-size.X, 0, 0),
            ElementOrigin.TopLeft => Matrix4.CreateTranslation(0, -size.Y, 0),
            ElementOrigin.TopRight => Matrix4.CreateTranslation(-size.X, -size.Y, 0),
            ElementOrigin.Center => Matrix4.CreateTranslation(-size.X / 2.0f, -size.Y / 2.0f, 0),
            _ => Matrix4.Identity
        };
        
        OnChange?.Invoke();
    }
    
    public void Translate(Vector3 position)
    {
        _translationMatrix = Matrix4.CreateTranslation(position);
        
        OnChange?.Invoke();
    }

    public void Rotate(Vector3 angle)
    {
        _rotationMatrix = Matrix4.CreateFromQuaternion(new Quaternion(angle));
        
        OnChange?.Invoke();
    }

    public void Scale(Vector3 scale)
    {
        _scaleMatrix = Matrix4.CreateScale(scale);
        
        OnChange?.Invoke();
    }

    public void Parent(Matrix4 parent)
    {
        _parentMatrix = parent;
        
        OnChange?.Invoke();
    }

    public Matrix4 GetMatrix()
    {
        _modelMatrix = _parentMatrix * (_originMatrix * _scaleMatrix * _rotationMatrix * _translationMatrix);
        return _modelMatrix;
    }
}