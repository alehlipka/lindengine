namespace lindengine.core.helpers
{
    internal static class FPSCounter
    {
        private static double _time;
        private static double _frames;
        private static int _oldFps;

        public static int FPS { get; private set; }
        public static int Max { get; private set; } = int.MinValue;
        public static int Min { get; private set; } = int.MaxValue;

        public delegate void FPSChangeDelegate();
        public static event FPSChangeDelegate? OnFPSChanged;

        public static void Calculate(double time)
        {
            _time += time;
            if (_time < 1.0)
            {
                _frames++;
                return;
            }

            FPS = (int)Math.Ceiling(_frames);

            _time = 0.0;
            _frames = 0.0;

            if (FPS > Max) Max = FPS;
            if (FPS < Min) Min = FPS;

            if (_oldFps == FPS) return;

            OnFPSChanged?.Invoke();

            _oldFps = FPS;
        }
    }
}
