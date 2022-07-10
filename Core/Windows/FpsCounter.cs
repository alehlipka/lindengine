using System;

namespace LindEngine.Core.Windows;

public static class FpsCounter
{
    private static double _time;
    private static double _frames;
    private static int _oldFps;
    private static int _oldMax;
    private static int _oldMin;

    public static int FPS { get; private set; }
    public static int Max { get; private set; } = int.MinValue;
    public static int Min { get; private set; } = int.MaxValue;
    public static bool IsChanged = false;

    public static void Calculate(double time)
    {
        

        _time += time;
        if (_time < 1.0)
        {
            _frames++;
        }
        else
        {
            IsChanged = false;
            FPS = (int)Math.Ceiling(_frames);

            _time = 0.0;
            _frames = 0.0;

            if (FPS > Max) Max = FPS;
            if (FPS < Min) Min = FPS;

            if (_oldFps != FPS) {
                _oldFps = FPS;
                IsChanged = true;
            }
        }
    }
}