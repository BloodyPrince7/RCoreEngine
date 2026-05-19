using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Graphics.OpenGL4;
using Engine.Graphics;

using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
namespace Engine.Core;

public class Game : GameWindow
{
    private Transform transform = new Transform();
    private Mesh triangleMesh;
    private Shader shader;
    private Matrix4 projection;
    private Camera camera = new Camera();
    private bool firstMove = true;

    private Vector2 lastPos;
    private float[] vertices =
{
    // Front Face
    -0.5f, -0.5f,  0.5f,
     0.5f, -0.5f,  0.5f,
     0.5f,  0.5f,  0.5f,

    -0.5f, -0.5f,  0.5f,
     0.5f,  0.5f,  0.5f,
    -0.5f,  0.5f,  0.5f,

    // Back Face
    -0.5f, -0.5f, -0.5f,
     0.5f,  0.5f, -0.5f,
     0.5f, -0.5f, -0.5f,

    -0.5f, -0.5f, -0.5f,
    -0.5f,  0.5f, -0.5f,
     0.5f,  0.5f, -0.5f,

    // Left Face
    -0.5f, -0.5f, -0.5f,
    -0.5f, -0.5f,  0.5f,
    -0.5f,  0.5f,  0.5f,

    -0.5f, -0.5f, -0.5f,
    -0.5f,  0.5f,  0.5f,
    -0.5f,  0.5f, -0.5f,

    // Right Face
     0.5f, -0.5f, -0.5f,
     0.5f,  0.5f,  0.5f,
     0.5f, -0.5f,  0.5f,

     0.5f, -0.5f, -0.5f,
     0.5f,  0.5f, -0.5f,
     0.5f,  0.5f,  0.5f,

    // Top Face
    -0.5f,  0.5f, -0.5f,
    -0.5f,  0.5f,  0.5f,
     0.5f,  0.5f,  0.5f,

    -0.5f,  0.5f, -0.5f,
     0.5f,  0.5f,  0.5f,
     0.5f,  0.5f, -0.5f,

    // Bottom Face
    -0.5f, -0.5f, -0.5f,
     0.5f, -0.5f,  0.5f,
    -0.5f, -0.5f,  0.5f,

    -0.5f, -0.5f, -0.5f,
     0.5f, -0.5f, -0.5f,
     0.5f, -0.5f,  0.5f
};

    private string vertexShaderSource = @"
#version 330 core

layout (location = 0) in vec3 aPosition;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    gl_Position =
        projection *
        view *
        model *
        vec4(aPosition, 1.0);
}";

    private string fragmentShaderSource = @"
#version 330 core

out vec4 FragColor;

void main()
{
    FragColor = vec4(0.2, 0.8, 0.7, 1.0);
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
        GL.Enable(EnableCap.DepthTest);
        CursorState = CursorState.Grabbed;
        float aspectRatio = Size.X / (float)Size.Y;
        // camera.Position.Z = 3f;
        projection = Matrix4.CreatePerspectiveFieldOfView(
            MathHelper.DegreesToRadians(45f),
            aspectRatio,
            0.1f,
            100f);

        shader = new Shader(vertexShaderSource, fragmentShaderSource);

        triangleMesh = new Mesh(vertices);
        transform.Position.X = 0.3f;
        transform.Scale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(
    ClearBufferMask.ColorBufferBit |
    ClearBufferMask.DepthBufferBit);

        shader.Use();
        Matrix4 model = transform.GetModelMatrix();
        Matrix4 view = camera.GetViewMatrix();

        shader.SetMatrix4("model", model);
        shader.SetMatrix4("view", view);
        shader.SetMatrix4("projection", projection);
        


        triangleMesh.Draw();

        SwapBuffers();
    }

    protected override void OnUnload()
    {
        base.OnUnload();

        shader.Delete();

        triangleMesh.Delete();
    }
    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        float cameraSpeed = 2.5f * (float)args.Time;

        if (KeyboardState.IsKeyDown(Keys.W))
            camera.Position += camera.Front * cameraSpeed;

        if (KeyboardState.IsKeyDown(Keys.S))
            camera.Position -= camera.Front * cameraSpeed;

        if (KeyboardState.IsKeyDown(Keys.A))
            camera.Position -= camera.Right * cameraSpeed;

        if (KeyboardState.IsKeyDown(Keys.D))
            camera.Position += camera.Right * cameraSpeed;
        var mouse = MouseState;

        if (firstMove)
        {
            lastPos = new Vector2(mouse.X, mouse.Y);

            firstMove = false;
        }
        else
        {
            float deltaX = mouse.X - lastPos.X;
            float deltaY = mouse.Y - lastPos.Y;

            lastPos = new Vector2(mouse.X, mouse.Y);

            float sensitivity = 0.1f;

            deltaX *= sensitivity;
            deltaY *= sensitivity;

            camera.Yaw += deltaX;
            camera.Pitch -= deltaY;

            camera.Pitch = MathHelper.Clamp(
                camera.Pitch,
                -89f,
                 89f);

            camera.UpdateVectors();
        }
    }

}