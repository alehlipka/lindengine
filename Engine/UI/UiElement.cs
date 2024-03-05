using Lindengine.Core;
using Lindengine.Graphics;
using Lindengine.Graphics.Shader;
using Lindengine.Output.Camera;
using Lindengine.Utilities;
using Lindengine.Utilities.BufferObject;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Lindengine.UI;

public class UiElement
{
    private Vector2i _size;
    private bool _isLoaded;
    private bool _isDebug;
    private Vector4 _border;
    private Texture? _texture;
    private readonly Texture? _boundingTexture;
    private readonly ModelMatrix _modelMatrix;
    private readonly BuffersContainer _buffersContainer;
    private readonly ShaderProgram _shader;
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
            _modelMatrix.SetOrigin(_modelMatrix.Origin, _size);
            uint[] indices;
            float[] vertices;
            if (_border == Vector4.Zero)
            {
                UtilityFunctions.GetVertices(_size, out indices, out vertices);
            }
            else
            {
                UtilityFunctions.GetBorderedVertices(_size, _border, out indices, out vertices);                
            }
            _buffersContainer.SetIndices(indices);
            _buffersContainer.SetVertices(vertices);
        }
    }
    public ElementOrigin Origin
    {
        get => _modelMatrix.Origin;
        set => _modelMatrix.SetOrigin(value, _size);
    }
    public Vector3 Position
    {
        get => _modelMatrix.Position;
        set => _modelMatrix.SetTranslation(value);
    }
    public Vector3 Angle
    {
        get => _modelMatrix.Angle;
        set => _modelMatrix.SetRotation(value);
    }
    public Vector3 Scale
    {
        get => _modelMatrix.Scale;
        set => _modelMatrix.SetScale(value);
    }
    public Texture? Texture
    {
        get => _texture;
        set => _texture = value;
    }
    public ShaderProgram Shader => _shader;
    public Vector4 Border
    {
        get => _border;
        set
        {
            _border = value;
            uint[] indices;
            float[] vertices;
            if (_border == Vector4.Zero)
            {
                UtilityFunctions.GetVertices(_size, out indices, out vertices);
            }
            else
            {
                UtilityFunctions.GetBorderedVertices(_size, _border, out indices, out vertices);                
            }
            _buffersContainer.SetIndices(indices);
            _buffersContainer.SetVertices(vertices);
        }
    }
    public bool IsDebug
    {
        get => _isDebug;
        set
        {
            _isDebug = value;
            _children.ForEach(child => child.IsDebug = _isDebug);
        }
    }

    public UiElement(Vector2i size, ShaderProgram shader)
    {
        _size = size;
        _border = Vector4.Zero;
        _texture = null;
        _boundingTexture = Lind.Engine.Resources.Load<Texture>(Path.Combine("Assets", "green.jpg"));
        _shader = shader;

        _isLoaded = false;
        _buffersContainer = new BuffersContainer();
        _modelMatrix = new ModelMatrix();
        
        _modelMatrix.SetOrigin(ElementOrigin.BottomLeft, _size);
        _modelMatrix.SetTranslation(Vector3.Zero);
        _modelMatrix.SetRotation(Vector3.Zero);
        _modelMatrix.SetScale(Vector3.One);
        
        _modelMatrix.ModelMatrixChanged += OnModelMatrixChanged;
    }

    private void OnModelMatrixChanged()
    {
        _children.ForEach(child => child._modelMatrix.SetParentMatrix(_modelMatrix.GetMatrix()));
    }

    public void AddChildren(UiElement children)
    {
        if (_children.Contains(children)) return;
        _children.Add(children);
        children._modelMatrix.SetParentMatrix(_modelMatrix.GetMatrix());
    }

    public void RemoveChildren(UiElement children)
    {
        children._modelMatrix.SetParentMatrix(Matrix4.Identity);
        _children.Remove(children);
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
        
        uint[] indices;
        float[] vertices;
        if (_border == Vector4.Zero)
        {
            UtilityFunctions.GetVertices(_size, out indices, out vertices);
        }
        else
        {
            UtilityFunctions.GetBorderedVertices(_size, _border, out indices, out vertices);                
        }
        _buffersContainer.SetVertices(vertices);
        _buffersContainer.SetIndices(indices);
        _buffersContainer.LinkShaderAttributes(_shader);
        
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
        
        _shader.Use();
        _texture?.Use();
        _shader.SetUniformData("viewMatrix", camera.ViewMatrix);
        _shader.SetUniformData("projectionMatrix", camera.ProjectionMatrix);
        _shader.SetUniformData("modelMatrix", _modelMatrix.GetMatrix());
        
        _buffersContainer.Draw();

        if (_isDebug)
        {
            GL.Disable(EnableCap.CullFace);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            _boundingTexture?.Use();
            _buffersContainer.Draw();
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.Enable(EnableCap.CullFace);
        }
        
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
        
        _texture?.Unload();
        _buffersContainer.Unload();
        
        _children.ForEach(child => child.Unload());

        _isLoaded = false;
    }

    protected virtual void OnLoad() { }
    protected virtual void OnWindowResize(Vector2i size) { }
    protected virtual void OnUpdate(double elapsedSeconds) { }
    protected virtual void OnRender(Camera camera, double elapsedSeconds) { }
    protected virtual void OnUnload() { }
}
