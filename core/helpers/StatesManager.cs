using lindengine.core.window;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using System.Reflection;

namespace lindengine.core.helpers
{
    internal static class StatesManager
    {
        private const string STATES_NAMESPACE = "lindengine.core.window.states";
        private const string STATES_NAME_CUT = "State";
        private static readonly List<State> _windowStates = [];
        private static State? _selectedState = null;

        public static void Create(Vector2i windowSize)
        {
            CreateStates(windowSize);
        }

        public static void Load(string stateName, Vector2i windowSize)
        {
            if (_selectedState?.Name != stateName || !_selectedState.IsLoaded)
            {
                _selectedState?.Unload();
                _selectedState = _windowStates.First(state => state.Name.Equals(stateName));
                _selectedState?.Load(windowSize);
            }
        }

        public static void Resize(ResizeEventArgs e)
        {
            _selectedState?.Resize(e);
        }

        public static void Update(FrameEventArgs args)
        {
            _selectedState?.Update(args);
        }

        public static void Render(FrameEventArgs args)
        {
            _selectedState?.Render(args);
        }

        public static void Unload()
        {
            _selectedState?.Unload();
        }

        private static void CreateStates(Vector2i windowSize)
        {
            _windowStates.Clear();

            List<Type> types = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .ToList()
                .FindAll(t => t.IsClass && t.Namespace == STATES_NAMESPACE);

            types.ForEach(type =>
            {
                string stateName = type.Name.Replace(STATES_NAME_CUT, "").ToLower();
                object? stateObject = Activator.CreateInstance(type, stateName, windowSize);
                if (stateObject != null)
                {
                    _windowStates.Add((State)stateObject);
                }
            });
        }
    }
}
