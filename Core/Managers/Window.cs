using System;
using System.Collections.Generic;
using LindEngine.Core.Windows;
using LindEngine.Core.Windows.Exceptions;
using OpenTK.Windowing.Desktop;

namespace LindEngine.Core.Managers;

/// <summary>
/// Windows manager class
/// </summary>
public class Window
{
    public static Window Manager { get; } = new();

    private readonly List<LindenWindow> _windowsList;
    private LindenWindow _selectedWindow;

    private Window()
    {
        _windowsList = new List<LindenWindow>();

        Type[] typelist = Application.Starter.GetTypesInNamespace("LindEngine.Game.Windows");
        for (int i = 0; i < typelist.Length; i++)
        {
            Console.WriteLine("Window detected: " + typelist[i].FullName);

            _windowsList.Add(
                (LindenWindow)
                Activator.CreateInstance(
                    typelist[i],
                    "main",
                    GameWindowSettings.Default,
                    new NativeWindowSettings() {
                        Title = "LindEngine"
                    }
                )
            );
        }
    }

    /// <summary>
    /// Set window as selected by name
    /// </summary>
    /// <param name="name">Window name</param>
    /// <exception cref="Exception">Window with that name is not exists</exception>
    public void SelectWindow(string name)
    {
        LindenWindow window = _windowsList.Find(window => window.Name == name);
        _selectedWindow = window ?? throw new WindowNotExistsException($"Window with name {name} is not exists");
    }

    /// <summary>
    /// Run selected window
    /// </summary>
    public void Run()
    {
        _selectedWindow.Run();
    }

    /// <summary>
    /// Close selected window
    /// </summary>
    public void Close()
    {
        _selectedWindow.Close();
    }
}