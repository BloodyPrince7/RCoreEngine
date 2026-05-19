using OpenTK.Graphics.OpenGL4;

namespace Engine.Graphics;

public class Mesh
{
    private int vao;
    private int vbo;
    private int vertexCount;

    private float[] vertices;

    public Mesh(float[] vertices)
    {
        this.vertices = vertices;
        vertexCount = vertices.Length / 3;

        SetupMesh();
    }

    private void SetupMesh()
    {
        vao = GL.GenVertexArray();
        vbo = GL.GenBuffer();

        GL.BindVertexArray(vao);

        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

        GL.BufferData(
            BufferTarget.ArrayBuffer,
            vertices.Length * sizeof(float),
            vertices,
            BufferUsageHint.StaticDraw);

        GL.VertexAttribPointer(
            0,
            3,
            VertexAttribPointerType.Float,
            false,
            3 * sizeof(float),
            0);

        GL.EnableVertexAttribArray(0);
    }

    public void Draw()
    {
        GL.BindVertexArray(vao);

        GL.DrawArrays(PrimitiveType.Triangles, 0, vertexCount);
    }

    public void Delete()
    {
        GL.DeleteBuffer(vbo);
        GL.DeleteVertexArray(vao);
    }
}