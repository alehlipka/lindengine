using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Lindengine.Graphics.Shader;

public class ShaderProgram
    {
        private readonly int _handle;
        private readonly Dictionary<string, int> _uniformLocations;

        /// <param name="handle">Shader program object handle</param>
        public ShaderProgram(int handle)
        {
            _handle = handle;

            GL.GetProgram(_handle, GetProgramParameterName.ActiveUniforms, out int numberOfUniforms);
            _uniformLocations = [];

            for (int i = 0; i < numberOfUniforms; i++)
            {
                string key = GL.GetActiveUniform(_handle, i, out _, out _);
                int location = GL.GetUniformLocation(_handle, key);

                _uniformLocations.Add(key, location);
            }
        }

        /// <summary>
        /// Use shader program
        /// </summary>
        public void Use()
        {
            GL.UseProgram(_handle);
        }

        /// <summary>
        /// Get shader program's attribute location
        /// </summary>
        /// <param name="attribName">Shader attribute variable name</param>
        /// <returns></returns>
        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(_handle, attribName);
        }

        /// <summary>
        /// Specify value of uniform value for shader program
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="NotImplementedException"></exception>
        public void SetUniformData<T>(string name, T data)
        {
            switch (data)
            {
                case Matrix4 value:
                    GL.UniformMatrix4(_uniformLocations[name], true, ref value);
                    break;
                case Vector3 value:
                    GL.Uniform3(_uniformLocations[name], value);
                    break;
                case int value:
                    GL.Uniform1(_uniformLocations[name], value);
                    break;
                case float value:
                    GL.Uniform1(_uniformLocations[name], value);
                    break;
                case double value:
                    GL.Uniform1(_uniformLocations[name], value);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Delete program and clear resources
        /// </summary>
        public void Unload()
        {
            GL.DeleteProgram(_handle);
        }
    }
