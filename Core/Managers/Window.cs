using System;
using System.Collections.Generic;
using LindEngine.Core.Exceptions;
using LindEngine.Core.Windows;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace LindEngine.Core.Managers;

/// <summary>
/// Windows manager class
/// </summary>
public class Window : LindenManager
{
    public static Window Manager { get; } = new();

    private readonly List<LindenWindow> _windowsList;
    public LindenWindow SelectedWindow { get; private set; }

    private Window()
    {
        _windowsList = new List<LindenWindow>();
        
        Console.WriteLine("Window manager created");

        AddItems();
    }

    protected sealed override void AddItems()
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
                        APIVersion = new Version(4, 6),
                        API = ContextAPI.OpenGL,
                        Profile = ContextProfile.Core,
                        StartFocused = true,
                        StartVisible = true,
                        IsEventDriven = false,
                        NumberOfSamples = 16,
                        Title = $"LindEngine",
                        WindowState = WindowState.Maximized,
                        MinimumSize = new Vector2i(800, 600)
                    }
                )
            );
            
            Console.WriteLine($"Window manager: window '{windowName}' added");
        }
    }
    
    public override List<string> GetNames()
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
    public override void Select(string name)
    {
        if (SelectedWindow?.Name == name) return;
        
        LindenWindow window = _windowsList.Find(window => window.Name == name);
        SelectedWindow = window ?? throw new WindowNotExistsException($"Window with name {name} is not exists");
        
        Console.WriteLine($"Window manager: window '{window.Name}' selected");
    }
}