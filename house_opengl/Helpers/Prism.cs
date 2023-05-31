using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Numerics;

public class TriangularPrism
{
    private int vao;
    private int vbo;
    private int ebo;

    private Vector3[] vertices;
    private ushort[] indices;

    public TriangularPrism(float x, float y, float z, float width, float height, float depth, string texturePath)
    {
        vertices = new Vector3[]
        {
            new Vector3(x - (width / 2), y - (height / 2), z - (depth / 2)),   // Vertex 0
            new Vector3(x + (width / 2), y - (height / 2), z - (depth / 2)),   // Vertex 1
            new Vector3(x + (width / 2), y - (height / 2), z + (depth / 2)),   // Vertex 2
            new Vector3(x - (width / 2), y - (height / 2), z + (depth / 2)),   // Vertex 3
            new Vector3(x, y + (height / 2), z)                              // Vertex 4 (top vertex)
        };

        indices = new ushort[]
        {
            0, 1, 2,    // Bottom triangle
            0, 2, 3,    // Bottom triangle
            0, 1, 4,    // Side triangle
            1, 2, 4,    // Side triangle
            2, 3, 4,    // Side triangle
            3, 0, 4     // Side triangle
        };

        // Inicjalizacja OpenGL

        vao = GL.GenVertexArray();
        GL.BindVertexArray(vao);

        vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float) * 3, vertices, BufferUsageHint.StaticDraw);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(float) * 3, 0);
        GL.EnableVertexAttribArray(0);

        ebo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(ushort), indices, BufferUsageHint.StaticDraw);

        GL.BindVertexArray(0);
    }

    public void Render()
    {
        GL.BindVertexArray(vao);
        GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedShort, 0);
        GL.BindVertexArray(0);
    }
}
