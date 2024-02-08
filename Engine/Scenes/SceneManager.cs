using Lindengine.Core.Interfaces;
using OpenTK.Mathematics;

namespace Lindengine.Scenes
{
    internal class SceneManager : IItemsManager<Scene>
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

        public void Load()
        {
            _selectedScene?.Load();
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

        public void LoadAll(Predicate<Scene> match)
        {
            _scenes.FindAll(match).ForEach(s => s.Load());
        }

        public void ResizeAll(Predicate<Scene> match, Vector2i size)
        {
            _scenes.ForEach(s => s.Resize(size, true));
        }

        public void UpdateAll(Predicate<Scene> match, double elapsedSeconds)
        {
            _scenes.ForEach(s => s.Update(elapsedSeconds, true));
        }

        public void RenderAll(Predicate<Scene> match, double elapsedSeconds)
        {
            _scenes.ForEach(s => s.Render(elapsedSeconds, true));
        }

        public void UnloadAll(Predicate<Scene> match)
        {
            _scenes.ForEach(s => s.Unload());
        }
    }
}
