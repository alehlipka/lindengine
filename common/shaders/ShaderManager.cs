namespace lindengine.common.shaders
{
    public static class ShaderManager
    {
        private static readonly List<Shader> _shaders = [];
        private static string _shadersPath = string.Empty;
        private static Shader? _selectedShader = null;

        public static void Create(string shadersPath)
        {
            _shadersPath = shadersPath;
            CreateShaders();
        }

        public static void Select(string shaderName)
        {
            if (_selectedShader?.Name != shaderName)
            {
                _selectedShader = _shaders.First(shader => shader.Name.Equals(shaderName));
                _selectedShader?.Use();
            }
        }

        public static int? GetAttribLocation(string attribName)
        {
            return _selectedShader?.GetAttribLocation(attribName);
        }

        public static void SetUniformData<T>(string name, T data)
        {
            _selectedShader?.SetUniformData(name, data);
        }

        public static void Unload()
        {
            _shaders.ForEach(shader => shader.Unload());
        }

        private static void CreateShaders()
        {
            _shaders.Clear();

            string[] directories = Directory.GetDirectories(_shadersPath, "*", SearchOption.TopDirectoryOnly);
            foreach (string directory in directories)
            {
                string shaderName = Path.GetFileName(directory);
                string vertexPath = Path.Combine(directory, "vertex.glsl");
                string fragmentPath = Path.Combine(directory, "fragment.glsl");
                
                _shaders.Add(new Shader(shaderName, vertexPath, fragmentPath));
            }
        }
    }
}
