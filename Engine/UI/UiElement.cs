using Lindengine.Graphics;
using Lindengine.Graphics.Shader;
using Lindengine.Output.Camera;
using Lindengine.Utilities;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Lindengine.UI;

public abstract class UiElement
{
    private Vector2i _size;
    private ElementOrigin _origin;
    private Vector3 _position;
    private Vector3 _angle;
    private Vector3 _scale;
    private Texture _texture;
    private readonly ShaderProgram _shader;
    private bool _isLoaded;
    private Matrix4 _originMatrix;
    private Matrix4 _translationMatrix;
    private Matrix4 _rotationMatrix;
    private Matrix4 _scaleMatrix;
    private Matrix4 _modelMatrix;
    private uint[] _indices;
    private float[] _vertices;
    private bool _isModelMatrixDirty;
    private bool _isVertexBufferDirty;
    private float _border;
    private readonly int _indexBufferName;
    private readonly int _vertexBufferName;
    private readonly int _vertexArrayName;
    private UiElement? _parent;
    private readonly List<UiElement> _children = [];

    private bool _isClickable;
    private bool _isDragable;
    
    private event VoidDelegate? LoadEvent;
    private event VoidDelegate? UnloadEvent;
    private event SizeDelegate? WindowResizeEvent;
    private event SecondsDelegate? UpdateEvent;
    private event CameraSecondsDelegate? RenderEvent;

    public Vector2i Size
    {
        get => _size;
        set
        {
            _size = value;
            _originMatrix = _origin switch
            {
                ElementOrigin.BottomLeft => Matrix4.CreateTranslation(0, 0, 0),
                ElementOrigin.BottomRight => Matrix4.CreateTranslation(-_size.X, 0, 0),
                ElementOrigin.TopLeft => Matrix4.CreateTranslation(0, -_size.Y, 0),
                ElementOrigin.TopRight => Matrix4.CreateTranslation(-_size.X, -_size.Y, 0),
                ElementOrigin.Center => Matrix4.CreateTranslation(-_size.X / 2.0f, -_size.Y / 2.0f, 0),
                _ => Matrix4.Identity
            };
            UtilityFunctions.GetBorderedVertices(_size, _border, out _indices, out _vertices);
            _isModelMatrixDirty = true;
            _isVertexBufferDirty = true;
        }
    }

    public ElementOrigin Origin
    {
        get => _origin;
        set
        {
            _origin = value;
            _originMatrix = _origin switch
            {
                ElementOrigin.BottomLeft => Matrix4.CreateTranslation(0, 0, 0),
                ElementOrigin.BottomRight => Matrix4.CreateTranslation(-_size.X, 0, 0),
                ElementOrigin.TopLeft => Matrix4.CreateTranslation(0, -_size.Y, 0),
                ElementOrigin.TopRight => Matrix4.CreateTranslation(-_size.X, -_size.Y, 0),
                ElementOrigin.Center => Matrix4.CreateTranslation(-_size.X / 2.0f, -_size.Y / 2.0f, 0),
                _ => Matrix4.Identity
            };
            _isModelMatrixDirty = true;
        }
    }

    public Vector3 Position
    {
        get => _position;
        set
        {
            _position = value;
            _translationMatrix = Matrix4.CreateTranslation(new Vector3(_position));
            _isModelMatrixDirty = true;
        }
    }

    public Vector3 Angle
    {
        get => _angle;
        set
        {
            _angle = value;
            _rotationMatrix = Matrix4.CreateFromQuaternion(new Quaternion(_angle));
            _isModelMatrixDirty = true;
        }
    }

    public Vector3 Scale
    {
        get => _scale;
        set
        {
            _scale = value;
            _scaleMatrix = Matrix4.CreateScale(_scale);
            _isModelMatrixDirty = true;
        }
    }

    public Texture Texture
    {
        get => _texture;
        set => _texture = value;
    }

    public ShaderProgram Shader => _shader;

    public float Border
    {
        get => _border;
        set
        {
            _border = value;
            UtilityFunctions.GetBorderedVertices(_size, _border, out _indices, out _vertices);
            _isVertexBufferDirty = true;
        }
    }

    public UiElement? Parent
    {
        get => _parent;
        set
        {
            _parent = value;
            _isModelMatrixDirty = true;
        }
    }

    protected UiElement(Vector2i size, float border, ElementOrigin origin, Vector3 position, Vector3 angle, Vector3 scale,
        Texture texture, ShaderProgram shader)
    {
        _size = size;
        _border = border;
        _origin = origin;
        _position = position;
        _angle = angle;
        _scale = scale;
        _texture = texture;
        _shader = shader;
        _parent = null;

        _isLoaded = false;
        _originMatrix = _origin switch
        {
            ElementOrigin.BottomLeft => Matrix4.CreateTranslation(0, 0, 0),
            ElementOrigin.BottomRight => Matrix4.CreateTranslation(-_size.X, 0, 0),
            ElementOrigin.TopLeft => Matrix4.CreateTranslation(0, -_size.Y, 0),
            ElementOrigin.TopRight => Matrix4.CreateTranslation(-_size.X, -_size.Y, 0),
            ElementOrigin.Center => Matrix4.CreateTranslation(-_size.X / 2.0f, -_size.Y / 2.0f, 0),
            _ => Matrix4.Identity
        };
        _translationMatrix = Matrix4.CreateTranslation(new Vector3(_position));
        _rotationMatrix = Matrix4.CreateFromQuaternion(new Quaternion(_angle));
        _scaleMatrix = Matrix4.CreateScale(_scale);

        _modelMatrix = _originMatrix * _scaleMatrix * _rotationMatrix * _translationMatrix;

        UtilityFunctions.GetBorderedVertices(_size, _border, out _indices, out _vertices);

        GL.CreateVertexArrays(1, out _vertexArrayName);
        GL.CreateBuffers(1, out _vertexBufferName);
        GL.CreateBuffers(1, out _indexBufferName);

        _isClickable = true;
    }

    public void AddElement(UiElement element)
    {
        element.Parent = this;
        if (_children.Contains(element)) return;
        _children.Add(element);
    }

    public void RemoveElement(UiElement element)
    {
        element.Parent = null;
        _children.Remove(element);
    }

    public void Load()
    {
        if (_isLoaded) return;

        LoadEvent += OnLoad;
        WindowResizeEvent += OnWindowResize;
        UpdateEvent += OnUpdate;
        RenderEvent += OnRender;
        UnloadEvent += OnUnload;

        LoadEvent?.Invoke();
        
        _children.ForEach(child => child.Load());

        _isLoaded = true;
    }

    public void WindowResize(Vector2i size, bool force = false)
    {
        if (!_isLoaded && !force) return;

        WindowResizeEvent?.Invoke(size);
        
        _children.ForEach(child => child.WindowResize(size, true));
    }

    public void Update(double elapsedSeconds, bool force = false)
    {
        if (!_isLoaded && !force) return;

        UpdateEvent?.Invoke(elapsedSeconds);

        if (_isModelMatrixDirty)
        {
            _modelMatrix = _originMatrix * _scaleMatrix * _rotationMatrix * _translationMatrix;
            if (_parent != null) _modelMatrix = _parent._modelMatrix * _modelMatrix;
            _isModelMatrixDirty = false;
            _children.ForEach(child => child._isModelMatrixDirty = true);
        }

        if (_isVertexBufferDirty)
        {
            GL.NamedBufferSubData(_vertexBufferName, 0, _vertices.Length * sizeof(float), _vertices);
            _isVertexBufferDirty = false;
        }
        
        _children.ForEach(child => child.Update(elapsedSeconds, true));
    }

    public void Render(Camera camera, double elapsedSeconds)
    {
        if (!_isLoaded) return;
        RenderEvent?.Invoke(camera, elapsedSeconds);
        
        _children.ForEach(child => child.Render(camera, elapsedSeconds));
    }

    public void Unload()
    {
        if (!_isLoaded) return;

        LoadEvent -= OnLoad;
        WindowResizeEvent -= OnWindowResize;
        UpdateEvent -= OnUpdate;
        RenderEvent -= OnRender;
        UnloadEvent?.Invoke();
        UnloadEvent -= OnUnload;
        
        _children.ForEach(child => child.Unload());

        _isLoaded = false;
    }

    protected virtual void OnLoad()
    {
        int positionAttribute = _shader.GetAttribLocation("aPosition");
        int textureAttribute = _shader.GetAttribLocation("aTexture");
        
        GL.NamedBufferData(_vertexBufferName, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);
        GL.NamedBufferData(_indexBufferName, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
        
        GL.EnableVertexArrayAttrib(_vertexArrayName, positionAttribute);
        GL.VertexArrayAttribBinding(_vertexArrayName, positionAttribute, 0);
        GL.VertexArrayAttribFormat(_vertexArrayName, positionAttribute, 3, VertexAttribType.Float, false, 0);
        
        GL.EnableVertexArrayAttrib(_vertexArrayName, textureAttribute);
        GL.VertexArrayAttribBinding(_vertexArrayName, textureAttribute, 0);
        GL.VertexArrayAttribFormat(_vertexArrayName, textureAttribute, 2, VertexAttribType.Float, false, 3 * sizeof(float));
        
        GL.VertexArrayVertexBuffer(_vertexArrayName, 0, _vertexBufferName, 0, 5 * sizeof(float));
        GL.VertexArrayElementBuffer(_vertexArrayName, _indexBufferName);
    }

    protected virtual void OnWindowResize(Vector2i size)
    {
    }

    protected virtual void OnUpdate(double elapsedSeconds)
    {
    }

    protected virtual void OnRender(Camera camera, double elapsedSeconds)
    {
        _shader.Use();
        _texture.Use();
        _shader.SetUniformData("viewMatrix", camera.ViewMatrix);
        _shader.SetUniformData("projectionMatrix", camera.ProjectionMatrix);
        _shader.SetUniformData("modelMatrix", _modelMatrix);

        GL.BindVertexArray(_vertexArrayName);
        GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
        GL.BindVertexArray(0);
    }

    protected virtual void OnUnload()
    {
        GL.DeleteBuffer(_vertexBufferName);
        GL.DeleteBuffer(_indexBufferName);
        GL.DeleteVertexArray(_vertexArrayName);
    }
}
