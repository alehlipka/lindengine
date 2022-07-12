using System;
using System.Collections.Generic;
using System.IO;
using LindEngine.Core.Exceptions;
using LindEngine.Core.Shaders;

namespace LindEngine.Core.Managers;

/// <summary>
/// Shaders manager class
/// </summary>
public class Shader
{
    public static Shader Manager { get; } = new();

    private readonly List<LindenShader> _shadersList;
    public LindenShader SelectedShader { get; private set; }

    private Shader()
    {
        _shadersList = new List<LindenShader>();

        Console.WriteLine("Shader manager created");

        AddShaders();
    }

    private void AddShaders()
    {
        string shadersDirectory = @"Data/Shaders";
        string[] directories = Directory.GetDirectories(shadersDirectory);
        foreach (string shaderName in directories)
        {
            _shadersList.Add(new LindenShader(shaderName));
            
            Console.WriteLine($"Shader manager: shader '{shaderName}' added");
        }
    }

    public List<string> GetShadersNames()
    {
        List<string> names = new List<string>();
        _shadersList.ForEach(shader => { names.Add(shader.Name); });

        return names;
    }

    public void SelectShader(string name)
    {
        if (SelectedShader?.Name == name) return;

        LindenShader shader = _shadersList.Find(shader => shader.Name == name);
        SelectedShader = shader ?? throw new ShaderNotExistsException($"Shader with name {name} is not exists");
        
        Console.WriteLine($"Shader manager: shader '{shader.Name}' selected");
    }
}