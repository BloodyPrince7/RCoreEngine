using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

using Engine.Graphics;
using Engine.ECS;

namespace Engine.Core;

public class Game : GameWindow
{
    // Scene
    private Scene scene = new Scene();

    // Main cube object
    private GameObject cube;

    // Graphics
    private Mesh cubeMesh;
    private Mesh prismMesh;
    private Shader shader;

    // Camera
    private Camera camera = new Camera();

    // Projection
    private Matrix4 projection;

    // Mouse look
    private bool firstMove = true;
    private Vector2 lastPos;

    // Cube vertices
    private float[] CubeVertices =
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
    private float[] prismVertices =
{
    // Front Triangle
     0f,   0.5f,  0.5f,
    -0.5f, -0.5f, 0.5f,
     0.5f, -0.5f, 0.5f,

    // Back Triangle
     0f,   0.5f, -0.5f,
     0.5f, -0.5f, -0.5f,
    -0.5f, -0.5f, -0.5f,

    // Left Face
     0f,   0.5f,  0.5f,
    -0.5f, -0.5f, 0.5f,
    -0.5f, -0.5f, -0.5f,

     0f,   0.5f,  0.5f,
    -0.5f, -0.5f, -0.5f,
     0f,   0.5f, -0.5f,

    // Right Face
     0f,   0.5f,  0.5f,
     0.5f, -0.5f, -0.5f,
     0.5f, -0.5f, 0.5f,

     0f,   0.5f,  0.5f,
     0.5f, -0.5f, -0.5f,
     0f,   0.5f, -0.5f,

    // Bottom Face
    -0.5f, -0.5f,  0.5f,
     0.5f, -0.5f,  0.5f,
     0.5f, -0.5f, -0.5f,

    -0.5f, -0.5f,  0.5f,
     0.5f, -0.5f, -0.5f,
    -0.5f, -0.5f, -0.5f
    };

    // Vertex Shader
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

    // Fragment Shader
    private string fragmentShaderSource = @"
#version 330 core

out vec4 FragColor;

void main()
{
    FragColor = vec4(0.2, 0.8, 0.7, 1.0);
}";

    public Game(
        GameWindowSettings gameSettings,
        NativeWindowSettings nativeSettings)
        : base(gameSettings, nativeSettings)
    {
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        // Background color
        GL.ClearColor(0.1f, 0.1f, 0.15f, 1.0f);

        // Enable depth testing
        GL.Enable(EnableCap.DepthTest);

        // Lock cursor
        CursorState = CursorState.Grabbed;

        // Projection
        float aspectRatio = Size.X / (float)Size.Y;

        projection =
            Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(45f),
                aspectRatio,
                0.1f,
                100f);

        // Shader
        shader = new Shader(
            vertexShaderSource,
            fragmentShaderSource);

        // Mesh
        cubeMesh = new Mesh(CubeVertices);

        // Create Cube GameObject
        cube = new GameObject("Cube");

        // Transform Component
        Transform cubeTransform =
            cube.AddComponent<Transform>();

        cubeTransform.Position =
            new Vector3(0f, 0f, 0f);

        cubeTransform.Scale =
            new Vector3(1f, 1f, 1f);

        // MeshRenderer Component
        MeshRenderer renderer =
            cube.AddComponent<MeshRenderer>();

        renderer.Mesh = cubeMesh;
        renderer.Shader = shader;

        // Add to Scene
        scene.Add(cube);
        prismMesh = new Mesh(prismVertices);

        GameObject prism =
            new GameObject("Prism");

        Transform prismTransform =
            prism.AddComponent<Transform>();

        prismTransform.Position =
            new Vector3(2f, 0f, 0f);

        MeshRenderer prismRenderer =
            prism.AddComponent<MeshRenderer>();

        prismRenderer.Mesh = prismMesh;
        prismRenderer.Shader = shader;

        scene.Add(prism);

        scene.Start();
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        float deltaTime = (float)args.Time;

        scene.Update(deltaTime);

        // ======================
        // Camera Movement
        // ======================

        float cameraSpeed = 2.5f * deltaTime;

        if (KeyboardState.IsKeyDown(Keys.W))
            camera.Position += camera.Front * cameraSpeed;

        if (KeyboardState.IsKeyDown(Keys.S))
            camera.Position -= camera.Front * cameraSpeed;

        if (KeyboardState.IsKeyDown(Keys.A))
            camera.Position -= camera.Right * cameraSpeed;

        if (KeyboardState.IsKeyDown(Keys.D))
            camera.Position += camera.Right * cameraSpeed;

        // ======================
        // Mouse Look
        // ======================

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

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(
            ClearBufferMask.ColorBufferBit |
            ClearBufferMask.DepthBufferBit);

        foreach (var obj in scene.GameObjects)
        {
            Transform transform =
                obj.GetComponent<Transform>();

            MeshRenderer renderer =
                obj.GetComponent<MeshRenderer>();

            if (transform != null &&
                renderer != null)
            {
                renderer.Shader.Use();

                Matrix4 model =
                    transform.GetModelMatrix();

                Matrix4 view =
                    camera.GetViewMatrix();

                renderer.Shader.SetMatrix4(
                    "model",
                    model);

                renderer.Shader.SetMatrix4(
                    "view",
                    view);

                renderer.Shader.SetMatrix4(
                    "projection",
                    projection);

                renderer.Mesh.Draw();
            }
        }

        SwapBuffers();
    }

    protected override void OnUnload()
    {
        base.OnUnload();

        shader.Delete();

        cubeMesh.Delete();
    }
}