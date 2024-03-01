using OpenTK.Mathematics;

namespace Lindengine.Scenes;

public class SceneManager
{
    private readonly List<Scene> _scenes = [];
    private Scene? _selectedScene;

    /// <summary>
    /// Add scene to manager's list
    /// </summary>
    /// <param name="scene"><see cref="Lindengine.Scenes.Scene"/> object</param>
    public void Add(Scene scene)
    {
        if (!_scenes.Exists(s => s.Name == scene.Name))
        {
            _scenes.Add(scene);
        }
    }

    /// <summary>
    /// Add many <see cref="Lindengine.Scenes.Scene"/> objects
    /// </summary>
    /// <param name="scenes">IEnumerable <see cref="Lindengine.Scenes.Scene"/> objects</param>
    public void AddMany(IEnumerable<Scene> scenes)
    {
        foreach (Scene scene in scenes)
        {
            Add(scene);
        }
    }

    /// <summary>
    /// Remove <see cref="Lindengine.Scenes.Scene"/> object from manager's list
    /// </summary>
    /// <param name="scene"><see cref="Lindengine.Scenes.Scene"/> object</param>
    /// <param name="withUnloading">Is scene must be unloaded before removing</param>
    public void Remove(Scene scene, bool withUnloading = true)
    {
        if (_selectedScene == scene)
        {
            _selectedScene = null;
        }

        if (withUnloading)
        {
            scene.Unload();
        }

        _scenes.Remove(scene);
    }

    /// <summary>
    /// Remove <see cref="Lindengine.Scenes.Scene"/> object from manager's list
    /// </summary>
    /// <param name="sceneName"><see cref="Lindengine.Scenes.Scene"/> name</param>
    /// <param name="withUnloading">Is scene must be unloaded before removing</param>
    public void Remove(string sceneName, bool withUnloading = true)
    {
        Scene? scene = _scenes.Find(s => s.Name == sceneName);
        if (scene != null)
        {
            Remove(scene, withUnloading);
        }
    }

    /// <summary>
    /// Remove many <see cref="Lindengine.Scenes.Scene"/> objects from manager's list
    /// </summary>
    /// <param name="scenes"><see cref="Lindengine.Scenes.Scene"/> object</param>
    /// <param name="withUnloading">Is scene must be unloaded before removing</param>
    public void RemoveMany(IEnumerable<Scene> scenes, bool withUnloading = true)
    {
        foreach (Scene scene in scenes)
        {
            Remove(scene, withUnloading);
        }
    }

    /// <summary>
    /// Select <see cref="Lindengine.Scenes.Scene"/> from manager's list and load it. Previously loaded state will be unloaded
    /// </summary>
    /// <param name="name">Name of <see cref="Lindengine.Scenes.Scene"/> object inside manager's list</param>
    public void Select(string name)
    {
        if (_selectedScene?.Name == name && _selectedScene.IsLoaded) return;
            
        _selectedScene?.Unload();
        _selectedScene = _scenes.Find(scene => scene.Name == name);
        _selectedScene?.Load();
    }

    /// <summary>
    /// Resize selected <see cref="Lindengine.Scenes.Scene"/> object
    /// </summary>
    /// <param name="size">New <see cref="Lindengine.Scenes.Scene"/> size</param>
    public void ResizeSelected(Vector2i size)
    {
        _selectedScene?.WindowResize(size);
    }

    /// <summary>
    /// Update selected <see cref="Lindengine.Scenes.Scene"/> object
    /// </summary>
    /// <param name="elapsedSeconds">How many seconds of time elapsed since the previous event</param>
    public void UpdateSelected(double elapsedSeconds)
    {
        _selectedScene?.Update(elapsedSeconds);
    }

    /// <summary>
    /// Render selected <see cref="Lindengine.Scenes.Scene"/> object
    /// </summary>
    /// <param name="elapsedSeconds">How many seconds of time elapsed since the previous event</param>
    public void RenderSelected(double elapsedSeconds)
    {
        _selectedScene?.Render(elapsedSeconds);
    }

    /// <summary>
    /// Unload selected <see cref="Lindengine.Scenes.Scene"/> object and clear resources
    /// </summary>
    public void UnloadSelected()
    {
        _selectedScene?.Unload();
    }

    /// <summary>
    /// Load all <see cref="Lindengine.Scenes.Scene"/> objects inside manager's list
    /// </summary>
    /// <param name="match">Condition predicate. Null is equal to all <see cref="Lindengine.Scenes.Scene"/> objects</param>
    public void LoadAll(Predicate<Scene>? match = null)
    {
        if (match != null)
        {
            _scenes.FindAll(match).ForEach(s => s.Load());
        }
        else
        {
            _scenes.ForEach(s => s.Load());
        }
    }

    /// <summary>
    /// Resize all <see cref="Lindengine.Scenes.Scene"/> objects inside manager's list
    /// </summary>
    /// <param name="size">New <see cref="Lindengine.Scenes.Scene"/> size</param>
    /// <param name="match">Condition predicate. Null is equal to all <see cref="Lindengine.Scenes.Scene"/> objects</param>
    public void ResizeAll(Vector2i size, Predicate<Scene>? match = null)
    {
        if (match != null)
        {
            _scenes.FindAll(match).ForEach(s => s.WindowResize(size, true));
        }
        else
        {
            _scenes.ForEach(s => s.WindowResize(size, true));
        }
    }

    /// <summary>
    /// Update all <see cref="Lindengine.Scenes.Scene"/> objects inside manager's list
    /// </summary>
    /// <param name="elapsedSeconds">How many seconds of time elapsed since the previous event</param>
    /// <param name="match">Condition predicate. Null is equal to all <see cref="Lindengine.Scenes.Scene"/> objects</param>
    public void UpdateAll(double elapsedSeconds, Predicate<Scene>? match = null)
    {
        if (match != null)
        {
            _scenes.FindAll(match).ForEach(s => s.Update(elapsedSeconds, true));
        }
        else
        {
            _scenes.ForEach(s => s.Update(elapsedSeconds, true));
        }
    }

    /// <summary>
    /// Render all <see cref="Lindengine.Scenes.Scene"/> objects inside manager's list
    /// </summary>
    /// <param name="elapsedSeconds">How many seconds of time elapsed since the previous event</param>
    /// <param name="match">Condition predicate. Null is equal to all <see cref="Lindengine.Scenes.Scene"/> objects</param>
    public void RenderAll(double elapsedSeconds, Predicate<Scene>? match = null)
    {
        if (match != null)
        {
            _scenes.FindAll(match).ForEach(s => s.Render(elapsedSeconds, true));
        }
        else
        {
            _scenes.ForEach(s => s.Render(elapsedSeconds, true));
        }
    }

    /// <summary>
    /// Unload all <see cref="Lindengine.Scenes.Scene"/> objects inside manager's list
    /// </summary>
    /// <param name="match">Condition predicate. Null is equal to all <see cref="Lindengine.Scenes.Scene"/> objects</param>
    public void UnloadAll(Predicate<Scene>? match = null)
    {
        if (match != null)
        {
            _scenes.FindAll(match).ForEach(s => s.Unload());
        }
        else
        {
            _scenes.ForEach(s => s.Unload());
        }
    }
}