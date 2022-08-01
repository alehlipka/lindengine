using System.Collections.Generic;
using LindEngine.Core.Managers;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace LindEngine.Core.Models.Meshes;

public struct Vertex
{
    public Vector3 Position;
    public Vector3 Normal;
    public Vector2 TexCoords;
    public Vector3 Tangent;
    public Vector3 Bitangent;
    public Vector4 BoneID;
    public Vector4 BoneWeight;

    public static int SizeInBytes() => Vector3.SizeInBytes * 4 + Vector2.SizeInBytes + Vector4.SizeInBytes * 2;
}

public class LindenMesh
{
    public Vector3 Position = Vector3.Zero;
    public Vector3 Rotation = Vector3.Zero;
    public Vector3 Scale = Vector3.One;
    public bool HasAnimations { get; set; }
    internal List<Vertex> vertices = new List<Vertex>();
    internal List<uint> indices = new List<uint>();

    private int VAO;
    private int VBO;
    private int EBO;

    public LindenMesh(bool hasAnimations)
    {
        HasAnimations = hasAnimations;
    }

    public Matrix4 TransformationMatrix()
    {
        return Matrix4.CreateScale(Scale) *
            Matrix4.CreateRotationX(Rotation.X) *
            Matrix4.CreateRotationY(Rotation.Y) *
            Matrix4.CreateRotationZ(Rotation.Z) *
            Matrix4.CreateTranslation(Position);
    }

    public void Initialize()
    {
        // VAO
        /*******************************************************************************************************************
        *   Сохраняет комбинацию состояний всех атрибутов данных вершины, сохраняет формат данных вершины
        *   и ссылку на VBO, требуемую данными вершины
        *   После выполнения привязки VAO все ваши конфигурации VBO позже становятся частью этого объекта VAO.
        *   Можно сказать, что VBO - это привязка информации об атрибутах вершины,
        *   а VAO - это привязка нескольких VBO
        *******************************************************************************************************************/
        VAO = GL.GenVertexArray();
        GL.BindVertexArray(VAO);

        // VBO
        /*******************************************************************************************************************
        *   Объект буфера вершин - это область буфера памяти, созданная в области памяти графической карты,
        *   которая используется для хранения различных типов информации атрибутов вершин, таких как координаты вершин,
        *   векторы вершин и данные цвета вершин. Во время рендеринга различные атрибутные данные вершин могут быть
        *   взяты непосредственно из VBO. Поскольку VBO находится в видеопамяти, а не в памяти, ему не нужно
        *   передавать данные из CPU, и эффективность обработки выше
        *******************************************************************************************************************/
        VBO = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Count * Vertex.SizeInBytes(), vertices.ToArray(), BufferUsageHint.StaticDraw);

        // Attributes
        // vertex.Positions
        int vertexPositionLocation = Shader.Manager.SelectedShader.GetAttribLocation("vertexPosition");
        GL.EnableVertexAttribArray(vertexPositionLocation);
        GL.VertexAttribPointer(vertexPositionLocation, 3, VertexAttribPointerType.Float, false, Vertex.SizeInBytes(), 0);
        // vertex.Normals
        int vertexNormalLocation = Shader.Manager.SelectedShader.GetAttribLocation("vertexNormal");
        GL.EnableVertexAttribArray(vertexNormalLocation);
        GL.VertexAttribPointer(vertexNormalLocation, 3, VertexAttribPointerType.Float, false, Vertex.SizeInBytes(), sizeof(float) * 3);
        // vertex.TexCoords
        int vertexTexturePositionLocation = Shader.Manager.SelectedShader.GetAttribLocation("vertexTexCoords");
        GL.EnableVertexAttribArray(vertexTexturePositionLocation);
        GL.VertexAttribPointer(vertexTexturePositionLocation, 2, VertexAttribPointerType.Float, false, Vertex.SizeInBytes(), sizeof(float) * 6);
        // vertex.Tangents
        int vertexTangentLocation = Shader.Manager.SelectedShader.GetAttribLocation("vertexTangent");
        GL.EnableVertexAttribArray(vertexTangentLocation);
        GL.VertexAttribPointer(vertexTangentLocation, 3, VertexAttribPointerType.Float, false, Vertex.SizeInBytes(), sizeof(float) * 8);
        // vertex.Bitangents
        int vertexBitangentLocation = Shader.Manager.SelectedShader.GetAttribLocation("vertexBitangent");
        GL.EnableVertexAttribArray(vertexBitangentLocation);
        GL.VertexAttribPointer(vertexBitangentLocation, 3, VertexAttribPointerType.Float, false, Vertex.SizeInBytes(), sizeof(float) * 11);
        if (HasAnimations)
        {
            // vertex.BoneId
            int vertexBoneIdLocation = Shader.Manager.SelectedShader.GetAttribLocation("vertexBoneId");
            GL.EnableVertexAttribArray(vertexBoneIdLocation);
            GL.VertexAttribPointer(vertexBoneIdLocation, 4, VertexAttribPointerType.Float, false, Vertex.SizeInBytes(), sizeof(float) * 14);
            // vertex.BoneWeight
            int vertexBoneWeightLocation = Shader.Manager.SelectedShader.GetAttribLocation("vertexBoneWeight");
            GL.EnableVertexAttribArray(vertexBoneWeightLocation);
            GL.VertexAttribPointer(vertexBoneWeightLocation, 4, VertexAttribPointerType.Float, false, Vertex.SizeInBytes(), sizeof(float) * 18);
        }

        // EBO
        /*******************************************************************************************************************
        *   Индексный буферный объект EBO эквивалентен концепции массива вершин в OpenGL,Чтобы решить проблему множественных
        *   повторных вызовов в одну и ту же вершину, можно уменьшить потери памяти и повысить эффективность., Когда нужны
        *   повторяющиеся вершины, эта вершина вызывается индексом позиции вершины.
        *   Содержимое, хранящееся в EBO, является индексом местоположения.
        *   EBO аналогично VBO, а также является частью буфера памяти в видеопамяти
        *******************************************************************************************************************/
        EBO = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Count * sizeof(uint), indices.ToArray(), BufferUsageHint.StaticDraw);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
    }
}
