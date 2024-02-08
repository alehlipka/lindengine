namespace Lindengine.Utilities;

internal class EventManager
{
    private readonly Dictionary<string, Action> sceneEventListeners = [];
    private readonly Dictionary<string, Action> inputEventListeners = [];

    public void AddSceneEventListener(string eventName, Action listener)
    {
        if (!sceneEventListeners.ContainsKey(eventName))
        {
            sceneEventListeners[eventName] = listener;
        }
        else
        {
            sceneEventListeners[eventName] += listener;
        }
    }

    public void AddInputEventListener(string eventName, Action listener)
    {
        if (!inputEventListeners.ContainsKey(eventName))
        {
            inputEventListeners[eventName] = listener;
        }
        else
        {
            inputEventListeners[eventName] += listener;
        }
    }

    public void TriggerSceneEvent(string eventName)
    {
        if (sceneEventListeners.TryGetValue(eventName, out Action? value))
        {
            value?.Invoke();
        }
    }

    public void TriggerInputEvent(string eventName)
    {
        if (inputEventListeners.TryGetValue(eventName, out Action? value))
        {
            value?.Invoke();
        }
    }
}
