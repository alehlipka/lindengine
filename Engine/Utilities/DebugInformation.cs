using System.Diagnostics;
using OpenTK.Graphics.OpenGL4;

namespace Lindengine.Utilities;

/// <summary>
/// Represents FPS and used memory information
/// </summary>
public class DebugInformation
{
    private int _fpsNow;
    private int _fpsMin = int.MaxValue;
    private int _fpsMax = int.MinValue;
    private double _time;
    private double _frames;
    private float _memoryUsed;
    private bool _isChanged;
    private readonly string _graphicsVendor = GL.GetString(StringName.Vendor);
    private readonly string _graphicsVersion = GL.GetString(StringName.Version);
    private readonly string _graphicsRenderer = GL.GetString(StringName.Renderer);
    private readonly string _graphicsGlslCompiler = GL.GetString(StringName.ShadingLanguageVersion);

    /// <summary>
    /// Calculate FPS information
    /// Must be placed inside draw method
    /// </summary>
    /// <param name="elapsedSeconds"></param>
    /// <returns></returns>
    public string CalculateDraw(double elapsedSeconds)
    {
        _time += elapsedSeconds;
        if (_time < 1.0)
        {
            _frames++;
            _isChanged = false;
        }
        else
        {
            _fpsNow = (int)Math.Ceiling(_frames);
            _time = 0.0;
            _frames = 0.0;
            if (_fpsNow > _fpsMax) _fpsMax = _fpsNow;
            if (_fpsNow < _fpsMin) _fpsMin = _fpsNow;
            _memoryUsed = Process.GetCurrentProcess().PrivateMemorySize64 / 1024f / 1024f;
            _isChanged = true;
        }

        return $"Lindengine Debug Information:\n" +
               $"Used memory: {_memoryUsed:0.000} MB\n" +
               $"FPS: {_fpsNow} Max: {_fpsMax} Min: {_fpsMin}\n" +
               $"Vendor: {_graphicsVendor}\n" +
               $"Version: {_graphicsVersion}\n" +
               $"Renderer: {_graphicsRenderer}\n" +
               $"GLSL compiler: {_graphicsGlslCompiler}";
    }

    /// <summary>
    /// Is some data changed
    /// </summary>
    /// <returns>true if true, otherwise false</returns>
    public bool IsChanged()
    {
        return _isChanged;
    }
}