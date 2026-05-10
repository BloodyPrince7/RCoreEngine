using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using Engine.Core;

class Program
{
    static void Main()
    {
        var gameSettings = GameWindowSettings.Default;

        var nativeSettings = new NativeWindowSettings()
        {
            ClientSize = new Vector2i(1280, 720),
            Title = "REngine"
        };

        using var game = new Game(gameSettings, nativeSettings);

        game.Run();
    }
}