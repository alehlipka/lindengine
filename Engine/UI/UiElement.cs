using Lindengine.Graphics;
using Lindengine.Graphics.Shader;
using Lindengine.Output.Camera;
using Lindengine.Utilities;
using Lindengine.Utilities.BufferObject;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Lindengine.UI;

public abstract class UiElement
{
    private ElementOrigin _origin;
    private Vector2i _size;
    private Vector3 _position;
    private Vector3 _angle;
    private Vector3 _scale;
    private bool _isLoaded;
    private uint[] _indices;
    private float[] _vertices;
    private float _border;
    private Texture _texture;
    private UiElement? _parent;
    private readonly ShaderProgram _shader;
    private readonly ModelMatrix _modelMatrix;
    private readonly BuffersContainer _buffersContainer;
    private readonly List<UiElement> _children = [];
    
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
            _modelMatrix.Origin(_origin, _size);
            UtilityFunctions.GetBorderedVertices(_size, _border, out _indices, out _vertices);
            _buffersContainer.SetVertices(_vertices);
        }
    }

    public ElementOrigin Origin
    {
        get => _origin;
        set
        {
            _origin = value;
            _modelMatrix.Origin(_origin, _size);
        }
    }

    public Vector3 Position
    {
        get => _position;
        set
        {
            _position = value;
            _modelMatrix.Translate(_position);
        }
    }

    public Vector3 Angle
    {
        get => _angle;
        set
        {
            _angle = value;
            _modelMatrix.Rotate(_angle);
        }
    }

    public Vector3 Scale
    {
        get => _scale;
        set
        {
            _scale = value;
            _modelMatrix.Scale(_scale);
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
            _buffersContainer.SetVertices(_vertices);
        }
    }

    public UiElement? Parent
    {
        get => _parent;
        set
        {
            _parent = value;
            if (_parent != null)
            {
                _modelMatrix.Parent(_parent._modelMatrix.GetMatrix());
                _parent._modelMatrix.OnChange += () => _modelMatrix.Parent(_parent._modelMatrix.GetMatrix());
            }
            else
            {
                _modelMatrix.Parent(Matrix4.Identity);
            }
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
        UtilityFunctions.GetBorderedVertices(_size, _border, out _indices, out _vertices);
        _buffersContainer = new BuffersContainer();
        _modelMatrix = new ModelMatrix();
        
        _modelMatrix.Origin(_origin, _size);
        _modelMatrix.Translate(_position);
        _modelMatrix.Rotate(_angle);
        _modelMatrix.Scale(_scale);
    }

    public void AddElement(UiElement element)
    {
        if (_children.Contains(element)) return;
        _children.Add(element);
        element.Parent = this;
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
        _buffersContainer.SetVertices(_vertices);
        _buffersContainer.SetIndices(_indices);
        _buffersContainer.LinkShaderAttributes(_shader);
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
        _shader.SetUniformData("modelMatrix", _modelMatrix.GetMatrix());
        _buffersContainer.Use();
        
        GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
    }

    protected virtual void OnUnload()
    {
        _texture.Unload();
        _buffersContainer.Unload();
    }
}
