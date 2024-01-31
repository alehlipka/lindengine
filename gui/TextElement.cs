using lindengine.common.cameras;
using lindengine.common.shaders;
using lindengine.common.textures;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using static StbTrueTypeSharp.StbTrueType;

namespace lindengine.gui
{
    public class TextElement : Element
    {
        protected byte[] bytes, bitmap;
        protected int l_h, b_cursor, ascent, descent, lineGap;
        protected float scale;
        protected stbtt_fontinfo info;
        protected string text;
        protected Texture texture;

        private int lineShift;

        public TextElement(string name, Vector2i size, string text) : base(name, size)
        {
            this.text = text;

            prepare();
            processText(text);
            saveImage();

            vertices = [
                0.0f,   0.0f,   0.0f, 0.0f, 0.0f,  // bottom left
                0.0f,   size.Y, 0.0f, 0.0f, 1.0f,  // top left
                size.X, size.Y, 0.0f, 1.0f, 1.0f,  // top right
                size.X, 0.0f,   0.0f, 1.0f, 0.0f,  // bottom right
            ];
            indices = [0, 3, 2, 0, 2, 1];

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

            texture = Texture.LoadFromFile("lindengine-logo-big", "assets/lindengine/lindengine-logo-big.png");
            modelMatrix = Matrix4.CreateTranslation(new Vector3(10, 100, 0));
        }

        protected override void OnRenderFrame(Element element, FrameEventArgs args)
        {
            GL.BindVertexArray(vertexArray);
            texture.Use();
            ShaderManager.Select("gui");
            CameraManager.Select(CameraType.Orthographic);
            ShaderManager.SetUniformData("viewMatrix", CameraManager.GetViewMatrix());
            ShaderManager.SetUniformData("projectionMatrix", CameraManager.GetProjectionMatrix());
            ShaderManager.SetUniformData("modelMatrix", modelMatrix);

            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
        }

        unsafe protected void prepare()
        {
            bytes = File.ReadAllBytes("assets/fonts/OpenSansBold.ttf");
            info = new();
            fixed (byte* ptr = bytes)
            {
                stbtt_InitFont(info, ptr, 0);
            }

            l_h = 24;

            bitmap = new byte[size.X * size.Y];
            scale = stbtt_ScaleForPixelHeight(info, l_h);
            b_cursor = 0;
            int asc, des, gap;
            stbtt_GetFontVMetrics(info, &asc, &des, &gap);
            ascent = (int)(asc * scale);
            descent = (int)(des * scale);
            lineGap = gap;

            lineShift = 0;
        }

        protected void processText(string text)
        {
            string[] lines = text.Split('\n');
            for (int line_number = 0; line_number < lines.Length; line_number++)
            {
                processLine(lines[line_number], line_number);
            }
        }

        protected void processLine(string line, int line_number)
        {
            b_cursor = size.X * l_h * (line_number + lineShift);

            string[] words = line.Split(' ');
            int currentLineWidth = 0;

            for (int word_number = 0; word_number < words.Length; word_number++)
            {
                int wordWidth = getTextWidth(words[word_number] + ' ');
                currentLineWidth += wordWidth;

                if (currentLineWidth >= size.X)
                {
                    lineShift++;
                    b_cursor = size.X * l_h * (line_number + lineShift);
                    currentLineWidth = wordWidth;
                }

                processWord(words[word_number]);
            }
        }

        protected void processWord(string word)
        {
            for (int char_number = 0; char_number < word.Length; char_number++)
            {
                char current_char = word[char_number];
                char? next_char = (char_number < word.Length - 1)
                    ? word[char_number + 1]
                    : null;

                processCharacter(current_char, next_char);
                if (next_char == null)
                {
                    processCharacter(' ');
                }
            }
        }

        unsafe protected int getTextWidth(string text)
        {
            int wordWidth = 0;

            int ax, lsb;
            foreach (char character in text)
            {
                stbtt_GetCodepointHMetrics(info, character, &ax, &lsb);
                wordWidth += (int)(ax * scale);
            }

            return wordWidth;
        }

        unsafe protected void processCharacter(char current_char, char? next_char = null)
        {
            /* how wide is this character */
            int ax, lsb;
            stbtt_GetCodepointHMetrics(info, current_char, &ax, &lsb);
            ax = (int)(ax * scale);
            lsb = (int)(lsb * scale);
            /* (Note that each Codepoint call has an alternative Glyph version which caches the work required to lookup the character word[i].) */

            /* get bounding box for character (may be offset to account for chars that dip above or below the line) */
            int c_x1, c_y1, c_x2, c_y2;
            stbtt_GetCodepointBitmapBox(info, current_char, scale, scale, &c_x1, &c_y1, &c_x2, &c_y2);
            int char_width = c_x2 - c_x1;
            int char_height = c_y2 - c_y1;

            /* compute y (different characters have different heights) */
            int y = ascent + c_y1;

            fixed (byte* ptr = bitmap)
            {
                /* render character (stride and offset is important here) */
                int byteOffset = b_cursor + lsb + (y * size.X);
                byte* ptr2 = ptr + byteOffset;
                stbtt_MakeCodepointBitmap(info, ptr2, char_width, char_height, size.X, scale, scale, current_char);
            }

            /* advance x */
            b_cursor += ax;

            /* add kerning */
            //int kern = 0;
            //if (next_char != null && next_char != '\n')
            //{
            //    kern = stbtt_GetCodepointKernAdvance(info, current_char, (char)next_char);
            //    b_cursor += (int)(kern * scale);
            //}
        }

        protected void saveImage()
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = "output.png";

            StbImageWriteSharp.ImageWriter imageWriter = new StbImageWriteSharp.ImageWriter();
            using FileStream stream = File.OpenWrite(Path.Combine(desktopPath, fileName));
            imageWriter.WritePng(bitmap, size.X, size.Y, StbImageWriteSharp.ColorComponents.Grey, stream);
        }
    }
}
