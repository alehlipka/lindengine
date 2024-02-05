using lindengine.common.cameras;
using lindengine.common.shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace lindengine.gui
{
    public class Element(string name, Vector2i size)
    {
        public readonly string Name = name.ToLower();

        protected int vertexBuffer;
        protected int indexBuffer;
        protected int vertexArray;
        protected uint[] indices = [];
        protected float[] vertices = [];
        protected List<Element> children = [];
        protected Vector2i size = size;
        protected Matrix4 modelMatrix = Matrix4.Identity;

        private delegate void ElementDelegate(Element element);
        private delegate void ElementLoadDelegate(Element element, Vector2i windowSize);
        private delegate void ElementContextResizeDelegate(Element element, ResizeEventArgs args);
        private delegate void ElementFrameDelegate(Element element, FrameEventArgs args);

        private event ElementLoadDelegate? LoadEvent;
        private event ElementDelegate? UnloadEvent;
        private event ElementContextResizeDelegate? ContextResizeEvent;
        private event ElementFrameDelegate? UpdateFrameEvent;
        private event ElementFrameDelegate? RendereFrameEvent;

        private bool _isLoaded;

        public void Load(Vector2i windowSize)
        {
            if (!_isLoaded)
            {
                LoadEvent += OnLoad;
                ContextResizeEvent += OnContextResize;
                UpdateFrameEvent += OnUpdateFrame;
                RendereFrameEvent += OnRenderFrame;
                UnloadEvent += OnUnload;

                LoadEvent?.Invoke(this, windowSize);

                InitializeVertices();
                InitializeIndices();
                InitializeBuffers();

                _isLoaded = true;
            }
        }

        protected virtual void InitializeVertices()
        {
            vertices = [
                0.0f,   0.0f,   0.0f, 0.0f, 0.0f,  // bottom left
                0.0f,   size.Y, 0.0f, 0.0f, 1.0f,  // top left
                size.X, size.Y, 0.0f, 1.0f, 1.0f,  // top right
                size.X, 0.0f,   0.0f, 1.0f, 0.0f,  // bottom right
            ];
        }

        protected virtual void InitializeIndices()
        {
            indices = [0, 3, 2, 0, 2, 1];
        }

        protected virtual void InitializeBuffers()
        {
            ShaderManager.Select("gui");
            int position_attribute = ShaderManager.GetAttribLocation("aPosition");
            int texture_attribute = ShaderManager.GetAttribLocation("aTexture");
            vertexBuffer = GL.GenBuffer();
            BindVertexBuffer();
            indexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(uint) * indices.Length, indices, BufferUsageHint.StaticDraw);
            // Generate a name for the array and create it.
            // Note that glGenVertexArrays() won't work here.
            GL.CreateVertexArrays(1, out vertexArray);
            // Instead of binding it, we pass it to the functions below.
            // Enable my attributes
            GL.EnableVertexArrayAttrib(vertexArray, position_attribute);
            GL.EnableVertexArrayAttrib(vertexArray, texture_attribute);
            // Set up the formats for my attributes
            GL.VertexArrayAttribFormat(
                vertexArray,
                position_attribute,     // attribute index, from the shader location = 1
                3,                      // size of attribute, vec3
                VertexAttribType.Float, // contains floats
                false,                  // does not need to be normalized as it is already, floats ignore this flag anyway
                0                       // relative offset after a Vector3
            );
            GL.VertexArrayAttribFormat(vertexArray, texture_attribute, 2, VertexAttribType.Float, false, 12);
            // Make my attributes all use binding 0
            GL.VertexArrayAttribBinding(vertexArray, position_attribute, 0);
            GL.VertexArrayAttribBinding(vertexArray, texture_attribute, 0);
            // Quickly bind all attributes to use "buffer"
            GL.VertexArrayVertexBuffer(vertexArray, 0, vertexBuffer, 0, 20);
            GL.VertexArrayElementBuffer(vertexArray, indexBuffer);
        }

        protected virtual void BindVertexBuffer()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
        }

        public void Resize(ResizeEventArgs e)
        {
            if (_isLoaded)
            {
                ContextResizeEvent?.Invoke(this, e);
            }
        }

        public void Update(FrameEventArgs args)
        {
            if (_isLoaded)
            {
                UpdateFrameEvent?.Invoke(this, args);
            }
        }

        public void Render(FrameEventArgs args)
        {
            if (_isLoaded)
            {
                RendereFrameEvent?.Invoke(this, args);

                ShaderManager.Select("gui");
                CameraManager.Select(CameraType.Orthographic);

                GL.BindVertexArray(vertexArray);
                ShaderManager.SetUniformData("viewMatrix", CameraManager.GetViewMatrix());
                ShaderManager.SetUniformData("projectionMatrix", CameraManager.GetProjectionMatrix());
                ShaderManager.SetUniformData("modelMatrix", modelMatrix);

                GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
            }
        }

        public void Unload()
        {
            if (_isLoaded)
            {
                UnloadEvent?.Invoke(this);

                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
                GL.DeleteBuffer(indexBuffer);
                GL.DeleteBuffer(vertexBuffer);
                GL.DeleteVertexArray(vertexArray);

                LoadEvent -= OnLoad;
                ContextResizeEvent -= OnContextResize;
                UpdateFrameEvent -= OnUpdateFrame;
                RendereFrameEvent -= OnRenderFrame;
                UnloadEvent -= OnUnload;

                _isLoaded = false;
            }
        }

        protected virtual void OnLoad(Element element, Vector2i windowSize) { }
        protected virtual void OnContextResize(Element element, ResizeEventArgs args) { }
        protected virtual void OnUpdateFrame(Element element, FrameEventArgs args) { }
        protected virtual void OnRenderFrame(Element element, FrameEventArgs args) { }
        protected virtual void OnUnload(Element element) { }
    }
}
