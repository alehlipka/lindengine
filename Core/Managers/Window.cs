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
    }

    /// <summary>
    /// Add new window if not exists.
    /// If this is a first window it will be selected as current
    /// </summary>
    /// <param name="name">Window name</param>
    /// <param name="gameWindowSettings"><see cref="T:OpenTK.Windowing.Desktop.GameWindow" /> settings</param>
    /// <param name="nativeWindowSettings"><see cref="T:OpenTK.Windowing.Desktop.NativeWindow" /> settings</param>
    public void AddWindow(string name, GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
    {
        if (_windowsList.Find(window => window.Name == name) == null)
        {
            _windowsList.Add(new LindenWindow(name, gameWindowSettings, nativeWindowSettings));
        }
        
        if (_selectedWindow == null)
        {
            SelectWindow(name);
        }
    }

    /// <summary>
    /// Remove window by name
    /// </summary>
    /// <param name="name">Window name</param>
    /// <exception cref="Exception">Selected window can't be removed</exception>
    public void RemoveWindow(string name)
    {
        LindenWindow window = _windowsList.Find(window => window.Name == name);
        if (window == null) return;
        
        if (_selectedWindow == window)
        {
            throw new RemoveWindowException("Selected window can't be removed");
        }
            
        _windowsList.Remove(window);
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