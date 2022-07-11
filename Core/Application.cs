using System;
using System.Linq;
using System.Reflection;
using LindEngine.Core.Managers;

namespace LindEngine.Core;

public class Application
{
    public static Application Starter { get; } = new();
    private Application() {}

    /// <summary>
    /// Start application instance
    /// </summary>
    public void Start()
    {
        Window.Manager.SelectWindow("Main");
        Window.Manager.SelectedWindow.SelectState("MainMenu");
        Window.Manager.SelectedWindow.Run();
    }

    public void Exit()
    {
        Environment.Exit(0);
    }

    public Type[] GetTypesInNamespace(string nameSpace)
    {
        return Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal))
            .ToArray();
    }
}