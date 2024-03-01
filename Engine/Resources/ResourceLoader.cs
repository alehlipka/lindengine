using Lindengine.Graphics;
using Lindengine.Graphics.Font;
using Lindengine.Graphics.Shader;
using Lindengine.Resources.Loader;
using OpenTK.Windowing.Common.Input;

namespace Lindengine.Resources;

public class ResourceLoader
{
    private readonly Dictionary<Type, Func<string, object>> _resourceLoaders = [];

    public ResourceLoader()
    {
        _resourceLoaders[typeof(Image)] = ImageLoader.LoadResource;
        _resourceLoaders[typeof(TextureData)] = TextureDataLoader.LoadResource;
        _resourceLoaders[typeof(Texture)] = TextureLoader.LoadResource;
        _resourceLoaders[typeof(ShaderProgram)] = ShaderProgramLoader.LoadResource;
        _resourceLoaders[typeof(Font)] = FontLoader.LoadResource;
    }

    /// <summary>
    /// Load resource
    /// </summary>
    /// <param name="path">Resource path</param>
    /// <typeparam name="T">Resource type</typeparam>
    /// <returns>Resource object</returns>
    /// <exception cref="ArgumentException">Unsupported resource type</exception>
    public T Load<T>(string path) where T : class
    {
        Type resourceType = typeof(T);
        _resourceLoaders.TryGetValue(resourceType, out Func<string, object>? value);

        return value != null
            ? (T)value(path)
            : throw new ArgumentException("Unsupported resource type");
    }
}