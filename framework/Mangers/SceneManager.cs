using System.Collections.Concurrent;
using Lindengine.Framework.Scenes;

namespace Lindengine.Framework.Mangers;

public class SceneManager
{
    private readonly ConcurrentDictionary<string, Scene> _scenes = new();
    private string? _currentSceneName;

    public void AddScene(string name, Scene scene)
    {
        if (_scenes.ContainsKey(name))
        {
            throw new ArgumentException($"Scene with name {name} is already exists.");
        }

        _scenes.TryAdd(name, scene);
    }

    public Scene GetCurrentScene()
    {
        if (_currentSceneName == null)
        {
            throw new ArgumentException("There is no current scene.");
        }
        
        if (!_scenes.TryGetValue(_currentSceneName, out Scene? value))
        {
            throw new ArgumentException($"Scene with name {_currentSceneName} is not exists.");
        }

        return value;
    }

    public void SelectScene(string name)
    {
        if (_scenes.ContainsKey(name))
        {
            if (_currentSceneName != null)
            {
                if (_scenes.TryGetValue(_currentSceneName, out Scene? scene))
                {
                    scene.Dispose();
                }
            }
            
            Interlocked.Exchange(ref _currentSceneName, name);
            _scenes[_currentSceneName].Initialize();
        }
        else
        {
            throw new ArgumentException($"Scene with name {name} is not exists.");
        }
    }
}