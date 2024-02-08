using Lindengine.Utilities;

namespace Lindengine.Core
{
    internal sealed class Engine
    {
        private static readonly Lazy<Engine> lazy = new(() => new Engine());
        public static Engine Instance { get => lazy.Value; }

        public EventManager eventManager = new();

        private Engine()
        {

        }
    }
}
