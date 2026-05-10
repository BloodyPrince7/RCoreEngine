using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Graphics.OpenGL4;
using Engine.Graphics;

using OpenTK.Mathematics;
namespace Engine.Core;

public class Game : GameWindow
{
    private Transform transform = new Transform();
    private Mesh triangleMesh;
    private Shader shader;

    private float[] vertices =
{
    // Triangle 1
    -0.5f, -0.5f,
     0.5f, -0.5f,
     0.5f,  0.5f,

    // Triangle 2
    -0.5f, -0.5f,
     0.5f,  0.5f,
    -0.5f,  0.5f
};

private string vertexShaderSource = @"
#version 330 core

layout (location = 0) in vec2 aPosition;

uniform mat4 model;

void main()
{
   vec4 pos = model * vec4(aPosition, 0.0, 1.0);

pos.x *= 720.0 / 1280.0;

gl_Position = pos;
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
        transform.Position.X = 0.3f;
        transform.Scale = new Vector3(0.5f);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        shader.Use();
        transform.Rotation.Z += 50f * (float)args.Time;
        Matrix4 model = transform.GetModelMatrix();
        
        shader.SetMatrix4("model", model);

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