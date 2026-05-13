using OpenTK.Mathematics;

namespace Engine.Core;

public class Camera
{
    public Vector3 Position = Vector3.Zero;

    public Matrix4 GetViewMatrix()
    {
        return Matrix4.CreateTranslation(-Position);
    }
}