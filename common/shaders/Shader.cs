using lindengine.common.logs;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace lindengine.common.shaders
{
    internal class Shader
    {
        public readonly string Name;
        protected readonly int Handle;
        protected readonly Dictionary<string, int> UniformLocations;

        public Shader(string name, string vertexPath, string fragmentPath)
        {
            Name = name;

            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, File.ReadAllText(vertexPath));
            CompileShader(vertexShader);

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, File.ReadAllText(fragmentPath));
            CompileShader(fragmentShader);

            Handle = GL.CreateProgram();

            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragmentShader);

            LinkProgram(Handle);

            GL.DetachShader(Handle, vertexShader);
            GL.DetachShader(Handle, fragmentShader);
            GL.DeleteShader(fragmentShader);
            GL.DeleteShader(vertexShader);

            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out int numberOfUniforms);
            UniformLocations = [];

            for (int i = 0; i < numberOfUniforms; i++)
            {
                string key = GL.GetActiveUniform(Handle, i, out _, out _);
                int location = GL.GetUniformLocation(Handle, key);

                UniformLocations.Add(key, location);
            }

            Logger.Write(LogLevel.Shader, $"Shader created: {Name}");
        }

        private static void CompileShader(int shader)
        {
            GL.CompileShader(shader);

            GL.GetShader(shader, ShaderParameter.CompileStatus, out int code);
            if (code != 1)
            {
                string infoLog = GL.GetShaderInfoLog(shader);
                throw new Exception($"Shader ({shader}) compiling error.\n\n{infoLog}");
            }
        }

        private static void LinkProgram(int program)
        {
            GL.LinkProgram(program);

            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int code);
            if (code != 1)
            {
                string infoLog = GL.GetProgramInfoLog(program);
                throw new Exception($"Program ({program}) linking error.\n\n{infoLog}");
            }
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(Handle, attribName);
        }

        public void SetUniformData<T>(string name, T data)
        {
            switch (data)
            {
                case Matrix4 value:
                    GL.UniformMatrix4(UniformLocations[name], true, ref value);
                    break;
                case Vector3 value:
                    GL.Uniform3(UniformLocations[name], value);
                    break;
                case int value:
                    GL.Uniform1(UniformLocations[name], value);
                    break;
                case float value:
                    GL.Uniform1(UniformLocations[name], value);
                    break;
                default:
                    throw new Exception("Shader set data not detected");
            }
        }

        public void Unload()
        {
            GL.DeleteProgram(Handle);
            Logger.Write(LogLevel.Shader, $"Shader unloaded: {Name}");
        }
    }
}
