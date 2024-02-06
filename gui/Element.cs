using lindengine.common.cameras;
using lindengine.common.shaders;
using lindengine.common.textures;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace lindengine.gui
{
    public class Element
    {
        public readonly string Name;
        public Vector2i Size { get; protected set; }

        protected int vertexBuffer;
        protected int indexBuffer;
        protected int vertexArray;
        protected uint[] indices = [];
        protected float[] vertices = [];
        protected List<Element> children = [];
        protected Matrix4 modelMatrix = Matrix4.Identity;
        protected List<Texture> textures = [];

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

        public Element(string name)
        {
            Name = name;

            GL.CreateBuffers(1, out vertexBuffer);
            GL.CreateBuffers(1, out indexBuffer);
            GL.CreateVertexArrays(1, out vertexArray);

            ShaderManager.Select("gui");
            int positionAttribute = ShaderManager.GetAttribLocation("aPosition");
            int textureAttribute = ShaderManager.GetAttribLocation("aTexture");

            GL.EnableVertexArrayAttrib(vertexArray, positionAttribute);
            GL.EnableVertexArrayAttrib(vertexArray, textureAttribute);
            GL.VertexArrayAttribFormat(vertexArray, positionAttribute, 3, VertexAttribType.Float, false, 0);
            GL.VertexArrayAttribFormat(vertexArray, textureAttribute, 2, VertexAttribType.Float, false, 12);
            GL.VertexArrayAttribBinding(vertexArray, positionAttribute, 0);
            GL.VertexArrayAttribBinding(vertexArray, textureAttribute, 0);
        }

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

        protected virtual void LoadTexture(Texture texture)
        {
            Texture? searchableTexture = textures.Find(t => t.Name == texture.Name);
            if (searchableTexture != null)
            {
                searchableTexture = texture;
            }
            else
            {
                textures.Add(texture);
            }
        }

        protected virtual void InitializeVertices()
        {
            vertices = [
                0.0f,   0.0f,   0.0f, 0.0f, 0.0f,  // bottom left
                0.0f,   Size.Y, 0.0f, 0.0f, 1.0f,  // top left
                Size.X, Size.Y, 0.0f, 1.0f, 1.0f,  // top right
                Size.X, 0.0f,   0.0f, 1.0f, 0.0f,  // bottom right
            ];
        }

        protected virtual void InitializeIndices()
        {
            indices = [0, 3, 2, 0, 2, 1];
        }

        protected virtual void InitializeBuffers()
        {
            GL.NamedBufferData(vertexBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.NamedBufferData(indexBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
            GL.VertexArrayVertexBuffer(vertexArray, 0, vertexBuffer, 0, 20);
            GL.VertexArrayElementBuffer(vertexArray, indexBuffer);
        }

        protected void ResetVertexBuffer()
        {
            GL.NamedBufferSubData(vertexBuffer, 0, vertices.Length * sizeof(float), vertices);
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

                textures.ForEach(texture => texture.Use());

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
