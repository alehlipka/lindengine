using System;
using System.Collections.Generic;
using System.IO;
using LindEngine.Core.Exceptions;
using LindEngine.Core.Shaders;

namespace LindEngine.Core.Managers;

/// <summary>
/// Shaders manager class
/// </summary>
public class Shader : LindenManager
{
    public static Shader Manager { get; } = new();

    private readonly List<LindenShader> _shadersList;
    public LindenShader SelectedShader { get; private set; }

    private Shader()
    {
        _shadersList = new List<LindenShader>();

        Console.WriteLine("Shader manager created");

        AddItems();
    }

    protected sealed override void AddItems()
    {
        string shadersDirectory = Path.Combine("Data", "Shaders");
        string[] directories = Directory.GetDirectories(shadersDirectory);
        foreach (string shaderName in directories)
        {
            _shadersList.Add(new LindenShader(shaderName));
            
            Console.WriteLine($"Shader manager: shader '{shaderName}' added");
        }
    }

    public override List<string> GetNames()
    {
        List<string> names = new List<string>();
        _shadersList.ForEach(shader => { names.Add(shader.Name); });

        return names;
    }

    public override void Select(string name)
    {
        if (SelectedShader?.Name == name) return;

        LindenShader shader = _shadersList.Find(shader => shader.Name == name);
        SelectedShader = shader ?? throw new ShaderNotExistsException($"Shader with name {name} is not exists");
        
        Console.WriteLine($"Shader manager: shader '{shader.Name}' selected");
    }

    public override void Dispose()
    {
        _shadersList.ForEach(s => s.Dispose());
    }
}