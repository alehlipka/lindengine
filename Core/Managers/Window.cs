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
    public LindenWindow SelectedWindow { get; private set; }

    private Window()
    {
        _windowsList = new List<LindenWindow>();

        AddWindows();
    }

    private void AddWindows()
    {
        Type[] typelist = Application.Starter.GetTypesInNamespace("LindEngine.Game.Windows");
        for (int i = 0; i < typelist.Length; i++)
        {
            Type type = typelist[i];
            string windowClassName = type.Name;
            string windowName = windowClassName.Split("Window")[0];

            _windowsList.Add(
                (LindenWindow)
                Activator.CreateInstance(
                    type,
                    windowName,
                    GameWindowSettings.Default,
                    new NativeWindowSettings() {
                        Title = $"LindEngine: {windowName} ({windowClassName})"
                    }
                )
            );
        }
    }

    public List<string> GetWindowsNames()
    {
        List<string> names = new List<string>();
        _windowsList.ForEach(window => { names.Add(window.Name); });

        return names;
    }

    /// <summary>
    /// Set window as selected by name
    /// </summary>
    /// <param name="name">Window name</param>
    /// <exception cref="WindowNotExistsException">Window with that name is not exists</exception>
    public void SelectWindow(string name)
    {
        LindenWindow window = _windowsList.Find(window => window.Name == name);
        SelectedWindow = window ?? throw new WindowNotExistsException($"Window with name {name} is not exists");
    }
}