using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Graphics.OpenGL4;

public class Game : GameWindow
{
    public Game(GameWindowSettings gameSettings,
                NativeWindowSettings nativeSettings)
        : base(gameSettings, nativeSettings)
    {
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        // Set background color
        GL.ClearColor(0.1f, 0.1f, 0.15f, 1.0f);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        // Clear screen
        GL.Clear(ClearBufferMask.ColorBufferBit);

        // Swap front/back buffers
        SwapBuffers();
    }
}

class Program
{
    static void Main()
    {
        var gameSettings = GameWindowSettings.Default;

        var nativeSettings = new NativeWindowSettings()
        {
            Size = new OpenTK.Mathematics.Vector2i(1280, 720),
            Title = "RcoreEngine"
        };

        using var game = new Game(gameSettings, nativeSettings);

        game.Run();
    }
}