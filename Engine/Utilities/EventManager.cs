namespace Lindengine.Utilities;

internal enum EventTarget
{
    Window,
    Scene
}

internal class EventManager
{
    private readonly Dictionary<string, Action> _windowEventListeners = [];
    private readonly Dictionary<string, Action> _sceneEventListeners = [];

    private Dictionary<string, Action> GetListeners(EventTarget target)
    {
        return target switch
        {
            EventTarget.Window => _windowEventListeners,
            EventTarget.Scene => _sceneEventListeners,

            _ => throw new NotImplementedException()
        };
    }

    internal void AddEventListener(EventTarget target, string eventName, Action listener)
    {
        Dictionary<string, Action> listeners = GetListeners(target);

        if (!listeners.ContainsKey(eventName))
        {
            listeners[eventName] = listener;
        }
        else
        {
            listeners[eventName] += listener;
        }
    }

    internal void TriggerEvent(EventTarget target, string eventName)
    {
        Dictionary<string, Action> listeners = GetListeners(target);

        if (listeners.TryGetValue(eventName, out Action? value))
        {
            value?.Invoke();
        }
    }
}
