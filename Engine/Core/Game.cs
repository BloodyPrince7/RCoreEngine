using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Graphics.OpenGL4;
using Engine.Graphics;

namespace Engine.Core;

public class Game : GameWindow
{
    private Mesh triangleMesh;
    private Shader shader;

    private float[] vertices =
    {
         0.0f,  0.5f,
        -0.5f, -0.5f,
         0.5f, -0.5f
    };

    private string vertexShaderSource = @"
#version 330 core

layout (location = 0) in vec2 aPosition;

void main()
{
    gl_Position = vec4(aPosition, 0.0, 1.0);
}";

    private string fragmentShaderSource = @"
#version 330 core

out vec4 FragColor;

void main()
{
    FragColor = vec4(0.2, 0.8, 0.3, 1.0);
}";

    public Game(GameWindowSettings gameSettings,
                NativeWindowSettings nativeSettings)
        : base(gameSettings, nativeSettings)
    {
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0.1f, 0.1f, 0.15f, 1.0f);

        shader = new Shader(vertexShaderSource, fragmentShaderSource);

        triangleMesh = new Mesh(vertices);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        shader.Use();

        triangleMesh.Draw();

        SwapBuffers();
    }

    protected override void OnUnload()
    {
        base.OnUnload();

        shader.Delete();

        triangleMesh.Delete();
    }
}