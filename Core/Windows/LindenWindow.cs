using System;
using System.Collections.Generic;
using LindEngine.Core.Exceptions;
using LindEngine.Core.Windows.States;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace LindEngine.Core.Windows;

/// <summary>
/// Based on <see cref="T:OpenTK.Windowing.Desktop.GameWindow" />
/// </summary>
public class LindenWindow: GameWindow
{
    /// <summary>
    /// Window name
    /// </summary>
    public readonly string Name;
    
    private readonly List<LindenWindowState> _states;
    public LindenWindowState SelectedState { get; private set; }

    protected LindenWindow(string name, GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
        VSync = VSyncMode.Off;
        Name = name;
        _states = new List<LindenWindowState>();

        Console.WriteLine($"Window created: {Name}");
        
        AddStates();
    }

    private void AddStates()
    {
        string stateNamespace = $"LindEngine.Game.States.{Name}WindowStates";

        Type[] typelist = Application.Starter.GetTypesInNamespace(stateNamespace);
        for (int i = 0; i < typelist.Length; i++)
        {
            Type type = typelist[i];
            string stateClassName = type.Name;
            string stateName = stateClassName.Split("State")[0];

            _states.Add((LindenWindowState)Activator.CreateInstance(type, stateName, this));
            
            Console.WriteLine($"Window {Name}: state '{stateName}' added");
        }
    }

    /// <summary>
    /// Set state as selected by name
    /// </summary>
    /// <param name="name">State name</param>
    /// <exception cref="StateNotExistsException">State with that name is not exists</exception>
    public void SelectState(string name)
    {
        if (SelectedState?.Name == name) return;
        
        LindenWindowState windowState = _states.Find(item => item.Name == name);
        SelectedState = windowState ?? throw new StateNotExistsException($"State with name {name} is not exists");
        
        Console.WriteLine($"Window '{Name}': state '{windowState.Name}' selected");
    }

    public List<string> GetStatesNames()
    {
        List<string> names = new List<string>();
        _states.ForEach(state => { names.Add(state.Name); });

        return names;
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        
        GL.Enable(EnableCap.Multisample);
        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        
        GL.ClearColor(Color4.Black);

        SelectedState?.OnLoad();
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
        
        SelectedState?.OnResize(e);
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);
        if (!IsFocused) return;

        SelectedState?.OnUpdate(args);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        FpsCounter.Calculate(args.Time);
        
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
        
        SelectedState?.OnRender(args);
        
        SwapBuffers();
    }

    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        base.OnMouseWheel(e);
        
        SelectedState?.OnMouseWheel(e);
    }

    protected override void OnTextInput(TextInputEventArgs e)
    {
        base.OnTextInput(e);
        
        SelectedState?.OnTextInput(e);
    }
}