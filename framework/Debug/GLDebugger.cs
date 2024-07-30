using System.Diagnostics;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;

namespace Lindengine.Framework.Debug;

// ReSharper disable once InconsistentNaming
internal static class GLDebugger
{
    [Conditional("DEBUG")]
    public static void Initialize()
    {
        GL.Enable(EnableCap.DebugOutput);
        GLDebugProc debugProc = DebugCallback;
        GL.DebugMessageCallback(debugProc, nint.Zero);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("OpenGL debug message callback initialized");
        Console.ResetColor();
    }

    private static void DebugCallback(DebugSource source, DebugType type, uint id, DebugSeverity severity, int length,
        nint message, nint userParam)
    {
        string messageString = Marshal.PtrToStringAnsi(message, length);
        ConsoleColor defaultColor = Console.ForegroundColor;
        Console.ForegroundColor = type switch
        {
            DebugType.DebugTypeError => ConsoleColor.Red,
            DebugType.DebugTypePerformance => ConsoleColor.Yellow,
            DebugType.DebugTypePortability => ConsoleColor.Blue,
            _ => defaultColor
        };

        Console.WriteLine($"[{source}] [{type}] [{severity}]");
        Console.WriteLine(messageString);
        Console.ResetColor();
    }
}