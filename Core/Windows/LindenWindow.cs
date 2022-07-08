using System;
using System.Collections.Generic;
using LindEngine.Core.Windows.States;
using LindEngine.Core.Windows.States.Exceptions;
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
    
    private List<LindenWindowState> _states;
    private LindenWindowState _selectedState;

    public LindenWindow(string name, GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
        Name = name;
        _states = new List<LindenWindowState>();

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
        }
    }

    /// <summary>
    /// Set state as selected by name
    /// </summary>
    /// <param name="name">State name</param>
    /// <exception cref="StateNotExistsException">State with that name is not exists</exception>
    public void SelectState(string name)
    {
        LindenWindowState windowState = _states.Find(item => item.Name == name);
        _selectedState = windowState ?? throw new StateNotExistsException($"State with name {name} is not exists");
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        
        GL.Enable(EnableCap.Multisample);
        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        
        GL.ClearColor(Color4.Black);

        _selectedState?.OnLoad();
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
        
        _selectedState?.OnResize(e);
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);
        if (!IsFocused) return;
        
        _selectedState?.OnUpdate(args);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        
        _selectedState?.OnRender(args);
        
        SwapBuffers();
    }
}