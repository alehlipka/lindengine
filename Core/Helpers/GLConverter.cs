using Assimp;
using OpenTK.Mathematics;

namespace LindEngine.Core.Helpers;

public static class GLConverter
{
    public static Matrix4 FromMatrix(Matrix4x4 mat)
    {
        Matrix4 m = new Matrix4();
        m.M11 = mat.A1;
        m.M12 = mat.A2;
        m.M13 = mat.A3;
        m.M14 = mat.A4;
        m.M21 = mat.B1;
        m.M22 = mat.B2;
        m.M23 = mat.B3;
        m.M24 = mat.B4;
        m.M31 = mat.C1;
        m.M32 = mat.C2;
        m.M33 = mat.C3;
        m.M34 = mat.C4;
        m.M41 = mat.D1;
        m.M42 = mat.D2;
        m.M43 = mat.D3;
        m.M44 = mat.D4;

        return m;
    }

    public static Vector3 FromVector3(Vector3D vector)
    {
        return new Vector3
        {
            X = vector.X,
            Y = vector.Y,
            Z = vector.Z
        };
    }

    public static Vector2 FromVector2(Vector2D vector)
    {
        return new Vector2
        {
            X = vector.X,
            Y = vector.Y
        };
    }

    public static OpenTK.Mathematics.Quaternion FromQuaternion(Assimp.Quaternion quaternion)
    {
        return new OpenTK.Mathematics.Quaternion
        {
            X = quaternion.X,
            Y = quaternion.Y,
            Z = quaternion.Z,
            W = quaternion.W
        };
    }
}
