using System.Diagnostics;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;

namespace Lindengine.Framework.Debug;

internal static class GLDebugger
{
    [Conditional("DEBUG")]
    public static void Initialize()
    {
        GL.Enable(EnableCap.DebugOutput);
        GLDebugProc debugProc = DebugCallback;
        GL.DebugMessageCallback(debugProc, nint.Zero);
        
        Console.WriteLine("OpenGL debug message callback initialized");
    }

    private static void DebugCallback(DebugSource source, DebugType type, uint id, DebugSeverity severity, int length,
        nint message, nint userParam)
    {
        string messageString = Marshal.PtrToStringAnsi(message, length);
        Console.WriteLine("OpenGL Debug Info");
        Console.WriteLine($"Source: {source}");
        Console.WriteLine($"Type: {type}");
        Console.WriteLine($"Severity: {severity}");
        Console.WriteLine(messageString);
    }
}