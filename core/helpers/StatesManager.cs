using lindengine.core.window;
using OpenTK.Windowing.Common;
using System.Reflection;

namespace lindengine.core.helpers
{
    internal static class StatesManager
    {
        private const string STATES_NAMESPACE = "lindengine.core.window.states";
        private const string STATES_NAME_CUT = "State";
        private static string SelectedStateName = string.Empty;
        private static readonly List<State> _windowStates = [];
        private static State? _selectedState = null;

        public static void Create()
        {
            CreateStates();
        }

        public static void Load(string stateName)
        {
            if (_selectedState?.Name != stateName)
            {
                if (_selectedState != null)
                {
                    SelectedStateName = stateName;
                    _selectedState.UnloadedEvent += OnStateUnloaded;
                    _selectedState.Unload();
                }
                else
                {
                    LoadStateByName(stateName);
                }
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

        private static void CreateStates()
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
                object? stateObject = Activator.CreateInstance(type, stateName);
                if (stateObject != null)
                {
                    _windowStates.Add((State)stateObject);
                }
            });
        }

        private static void LoadStateByName(string stateName)
        {
            _selectedState = _windowStates.First(state => state.Name.Equals(stateName));
            _selectedState?.Load();
        }

        private static void OnStateUnloaded(State state)
        {
            if (_selectedState != null)
            {
                _selectedState.UnloadedEvent -= OnStateUnloaded;
            }
            LoadStateByName(SelectedStateName);
        }
    }
}
