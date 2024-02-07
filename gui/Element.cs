using lindengine.common.cameras;
using lindengine.common.shaders;
using lindengine.common.textures;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace lindengine.gui
{
    public class Element(string name)
    {
        public readonly string Name = name;
        public Vector2i Size { get; protected set; }
        public bool IsLoaded { get; protected set; }

        protected int vertexBufferName;
        protected int indexBufferName;
        protected int vertexArrayName;
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

        public void Load(Vector2i windowSize)
        {
            if (!IsLoaded)
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

                IsLoaded = true;
            }
        }

        protected virtual void LoadTexture(string name, TextureData textureData, bool mipmap = false, TextureUnit unit = TextureUnit.Texture0)
        {
            Texture? searchableTexture = textures.Find(t => t.Name == name);
            if (searchableTexture == null)
            {
                textures.Add(Texture.LoadFromBytes(name, textureData, mipmap, unit));
            }
        }

        protected virtual void ChangeTexture(string name, TextureData textureData)
        {
            Texture? searchableTexture = textures.Find(t => t.Name == name);
            searchableTexture?.Change(textureData);
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
            indexBufferName = GL.GenBuffer();
            vertexBufferName = GL.GenBuffer();
            vertexArrayName = GL.GenVertexArray();

            GL.BindVertexArray(vertexArrayName);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferName);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBufferName);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(float), indices, BufferUsageHint.StaticDraw);

            ShaderManager.Select("gui");
            int positionAttribute = ShaderManager.GetAttribLocation("aPosition");
            int textureAttribute = ShaderManager.GetAttribLocation("aTexture");

            GL.EnableVertexAttribArray(positionAttribute);
            GL.VertexAttribPointer(positionAttribute, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            GL.EnableVertexAttribArray(textureAttribute);
            GL.VertexAttribPointer(textureAttribute, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

        }

        protected void ResetVertexBuffer()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferName);
            GL.BufferSubData(BufferTarget.ArrayBuffer, 0, vertices.Length * sizeof(float), vertices);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void Resize(ResizeEventArgs e)
        {
            if (IsLoaded)
            {
                ContextResizeEvent?.Invoke(this, e);
            }
        }

        public void Update(FrameEventArgs args)
        {
            if (IsLoaded)
            {
                UpdateFrameEvent?.Invoke(this, args);
            }
        }

        public void Render(FrameEventArgs args)
        {
            if (IsLoaded)
            {
                RendereFrameEvent?.Invoke(this, args);

                textures.ForEach(texture => texture.Use());

                ShaderManager.Select("gui");
                CameraManager.Select(CameraType.Orthographic);

                ShaderManager.SetUniformData("viewMatrix", CameraManager.GetViewMatrix());
                ShaderManager.SetUniformData("projectionMatrix", CameraManager.GetProjectionMatrix());
                ShaderManager.SetUniformData("modelMatrix", modelMatrix);

                GL.BindVertexArray(vertexArrayName);
                GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
                GL.BindVertexArray(0);
            }
        }

        public void Unload()
        {
            if (IsLoaded)
            {
                UnloadEvent?.Invoke(this);

                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
                GL.BindVertexArray(0);

                GL.DeleteBuffer(vertexBufferName);
                GL.DeleteBuffer(indexBufferName);
                GL.DeleteVertexArray(vertexArrayName);

                LoadEvent -= OnLoad;
                ContextResizeEvent -= OnContextResize;
                UpdateFrameEvent -= OnUpdateFrame;
                RendereFrameEvent -= OnRenderFrame;
                UnloadEvent -= OnUnload;

                IsLoaded = false;
            }
        }

        protected virtual void OnLoad(Element element, Vector2i windowSize) { }
        protected virtual void OnContextResize(Element element, ResizeEventArgs args) { }
        protected virtual void OnUpdateFrame(Element element, FrameEventArgs args) { }
        protected virtual void OnRenderFrame(Element element, FrameEventArgs args) { }
        protected virtual void OnUnload(Element element) { }
    }
}
