using Lindengine.Core;
using OpenTK.Mathematics;

namespace Lindengine.Scenes
{
    public class SceneManager() : IManager<Scene>
    {
        private readonly List<Scene> _scenes = [];
        private Scene? _selectedScene;

        public void Add(Scene scene)
        {
            if (!_scenes.Exists(s => s.Name == scene.Name))
            {
                _scenes.Add(scene);
            }
        }

        public void AddMany(IEnumerable<Scene> scenes)
        {
            foreach (Scene scene in scenes)
            {
                Add(scene);
            }
        }

        public void Remove(Scene scene, bool withUnloading = true)
        {
            if (_selectedScene == scene)
            {
                _selectedScene = null;
            }

            if (withUnloading && scene.IsLoaded)
            {
                scene.Unload();
            }

            _scenes.Remove(scene);
        }

        public void Remove(string sceneName, bool withUnloading = true)
        {
            Scene? scene = _scenes.Find(s => s.Name == sceneName);
            if (scene != null)
            {
                Remove(scene, withUnloading);
            }
        }

        public void RemoveMany(IEnumerable<Scene> scenes, bool withUnloading = true)
        {
            foreach (Scene scene in scenes)
            {
                Remove(scene, withUnloading);
            }
        }

        public Scene? Select(string name)
        {
            _selectedScene = _scenes.Find(scene => scene.Name == name);
            return _selectedScene;
        }

        public void Resize(Vector2i size)
        {
            _selectedScene?.Resize(size);
        }

        public void Update(double elapsedSeconds)
        {
            _selectedScene?.Update(elapsedSeconds);
        }

        public void Render(double elapsedSeconds)
        {
            _selectedScene?.Render(elapsedSeconds);
        }

        public void Unload()
        {
            _selectedScene?.Unload();
        }

        public void ResizeAll(Vector2i size)
        {
            _scenes.ForEach(s => s.Resize(size));
        }

        public void UpdateAll(double elapsedSeconds)
        {
            _scenes.ForEach(s => s.Update(elapsedSeconds));
        }

        public void RenderAll(double elapsedSeconds)
        {
            _scenes.ForEach(s => s.Render(elapsedSeconds));
        }

        public void UnloadAll()
        {
            _scenes.ForEach(s => s.Unload());
        }
    }
}
