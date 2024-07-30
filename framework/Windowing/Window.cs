using Lindengine.Framework.Debug;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Lindengine.Framework.Windowing;

public class Window : GameWindow
{
    public Window(string title = "Lindengine Demo", bool isVSyncEnabled = false)
        : base(GameWindowSettings.Default, new NativeWindowSettings()
        {
            APIVersion = new Version(4, 6),
            Title = title,
            Vsync = isVSyncEnabled ? VSyncMode.On : VSyncMode.Off
        })
    {
        GLDebugger.Initialize();
        
        int VAO = GL.CreateVertexArray();
        GL.BindVertexArray(VAO);

        string vertexShaderCode = @"
        #version 460 core
        out vec3 color;
        const vec2 pos[3] = vec2[3] (
            vec2(-0.6, -0.4),
            vec2(0.6, -0.4),
            vec2(0.0, 0.6)
        );
        const vec3 col[3] = vec3[3] (
            vec3(1.0, 0.0, 0.0),
            vec3(0.0, 1.0, 0.0),
            vec3(0.0, 0.0, 1.0)
        );
        void main() {
            gl_Position = vec4(pos[gl_VertexID], 0.0, 1.0);
            color = col[gl_VertexID];
        }
        ";
        
        string fragmentShaderCode = @"
        #version 460 core
        in vec3 color;
        out vec4 out_FragColor;
        void main() {
            out_FragColor = vec4(color, 1.0);
        }
        ";

        int vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, vertexShaderCode);
        GL.CompileShader(vertexShader);
        int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, fragmentShaderCode);
        GL.CompileShader(fragmentShader);
        int shaderProgram = GL.CreateProgram();
        GL.AttachShader(shaderProgram, vertexShader);
        GL.AttachShader(shaderProgram, fragmentShader);
        GL.LinkProgram(shaderProgram);
        GL.UseProgram(shaderProgram);
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        GL.Viewport(0, 0, e.Width, e.Height);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        GL.ClearColor(Color4.Black);
        GL.Clear(ClearBufferMask.ColorBufferBit);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
        SwapBuffers();
    }
}