using Lindengine.Output.Camera;
using OpenTK.Mathematics;

namespace Lindengine.Utilities;

internal delegate void VoidDelegate();
internal delegate void SizeDelegate(Vector2i size);
internal delegate void SecondsDelegate(double elapsedSeconds);
internal delegate void CameraSecondsDelegate(Camera camera, double elapsedSeconds);
