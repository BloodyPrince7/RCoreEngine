using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
namespace Engine.Graphics;

public class Shader
{
    public int Handle;

    public Shader(string vertexShaderSource, string fragmentShaderSource)
    {
        // Vertex Shader
        int vertexShader = GL.CreateShader(ShaderType.VertexShader);

        GL.ShaderSource(vertexShader, vertexShaderSource);
        GL.CompileShader(vertexShader);

        // Fragment Shader
        int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);

        GL.ShaderSource(fragmentShader, fragmentShaderSource);
        GL.CompileShader(fragmentShader);

        // Shader Program
        Handle = GL.CreateProgram();

        GL.AttachShader(Handle, vertexShader);
        GL.AttachShader(Handle, fragmentShader);

        GL.LinkProgram(Handle);

        // Cleanup
        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);
    }

    public void Use()
    {
        GL.UseProgram(Handle);
    }

    public void Delete()
    {
        GL.DeleteProgram(Handle);
    }
    public void SetMatrix4(string name, Matrix4 matrix)
    {
        int location = GL.GetUniformLocation(Handle, name);

        GL.UniformMatrix4(location, false, ref matrix);
    }   
}