using lindengine.common.cameras;
using lindengine.common.shaders;
using lindengine.common.textures;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace lindengine.gui
{
    public class TextElement : Element
    {
        protected string text;
        protected int fontSize = 24;
        protected Texture? texture;

        public TextElement(string name, Vector2i size, string text) : base(name, size)
        {
            this.text = text;
        }

        public void SetText(string newText)
        {
            if (!text.Equals(newText))
            {
                text = newText;
                byte[] textBytes = BitmapText.GetBytes("assets/fonts/OpenSansBold.ttf", size, text, fontSize);
                texture = Texture.LoadFromBytes($"{Name}_texture", textBytes, size);
            }
        }

        protected override void OnLoad(Element element)
        {
            vertices = [
                0.0f,   0.0f,   0.0f, 0.0f, 0.0f,  // bottom left
                0.0f,   size.Y, 0.0f, 0.0f, 1.0f,  // top left
                size.X, size.Y, 0.0f, 1.0f, 1.0f,  // top right
                size.X, 0.0f,   0.0f, 1.0f, 0.0f,  // bottom right
            ];
            indices = [0, 3, 2, 0, 2, 1];

            byte[] textBytes = BitmapText.GetBytes("assets/fonts/OpenSansBold.ttf", size, text, fontSize);
            texture = Texture.LoadFromBytes($"{Name}_texture", textBytes, size);

            ShaderManager.Select("gui");
            int position_attribute = ShaderManager.GetAttribLocation("aPosition");
            int texture_attribute = ShaderManager.GetAttribLocation("aTexture");

            vertexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
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

        protected override void OnContextResize(Element element, ResizeEventArgs args)
        {
            vertices = [
                0.0f,   0.0f,   0.0f, 0.0f, 0.0f,  // bottom left
                0.0f,   size.Y, 0.0f, 0.0f, 1.0f,  // top left
                size.X, size.Y, 0.0f, 1.0f, 1.0f,  // top right
                size.X, 0.0f,   0.0f, 1.0f, 0.0f,  // bottom right
            ];

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            byte[] textBytes = BitmapText.GetBytes("assets/fonts/OpenSansBold.ttf", size, text, 24);
            texture = Texture.LoadFromBytes($"{Name}_texture", textBytes, size);

            modelMatrix = Matrix4.CreateTranslation(Vector3.Zero);
        }

        protected override void OnRenderFrame(Element element, FrameEventArgs args)
        {
            texture?.Use();
            ShaderManager.Select("gui");
            CameraManager.Select(CameraType.Orthographic);

            GL.BindVertexArray(vertexArray);
            ShaderManager.SetUniformData("viewMatrix", CameraManager.GetViewMatrix());
            ShaderManager.SetUniformData("projectionMatrix", CameraManager.GetProjectionMatrix());
            ShaderManager.SetUniformData("modelMatrix", modelMatrix);

            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
        }
    }
}
