using Lindengine.UI;
using OpenTK.Mathematics;

namespace Lindengine.Utilities;

public class ModelMatrix
{
    public ElementOrigin Origin => _origin;
    public Vector3 Position => _position;
    public Vector3 Angle => _angle;
    public Vector3 Scale => _scale;
    
    private ElementOrigin _origin;
    private Vector3 _position;
    private Vector3 _angle;
    private Vector3 _scale;
    private Matrix4 _originMatrix = Matrix4.Identity;
    private Matrix4 _translationMatrix = Matrix4.Identity;
    private Matrix4 _rotationMatrix = Matrix4.Identity;
    private Matrix4 _scaleMatrix = Matrix4.Identity;
    private Matrix4 _parentMatrix = Matrix4.Identity;
    
    internal event VoidDelegate? ModelMatrixChanged;

    public void SetOrigin(ElementOrigin origin, Vector2i size)
    {
        _origin = origin;
        _originMatrix = _origin switch
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
        _position = position;
        _translationMatrix = Matrix4.CreateTranslation(_position);
        
        ModelMatrixChanged?.Invoke();
    }

    public void SetRotation(Vector3 angle)
    {
        _angle = angle;
        _rotationMatrix = Matrix4.CreateFromQuaternion(new Quaternion(_angle));
        
        ModelMatrixChanged?.Invoke();
    }

    public void SetScale(Vector3 scale)
    {
        _scale = scale;
        _scaleMatrix = Matrix4.CreateScale(_scale);
        
        ModelMatrixChanged?.Invoke();
    }

    public void SetParentMatrix(Matrix4 parent)
    {
        _parentMatrix = parent;
        
        ModelMatrixChanged?.Invoke();
    }

    public Matrix4 GetMatrix()
    {
        return _parentMatrix * (_originMatrix * _scaleMatrix * _rotationMatrix * _translationMatrix);
    }
}