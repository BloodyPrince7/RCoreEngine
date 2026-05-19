using OpenTK.Mathematics;

namespace Engine.Core;

public class Camera
{
    public Vector3 Position = new Vector3(0f, 0f, 3f);

    public float Pitch = 0f;
    public float Yaw = -90f;

    public Vector3 Front = -Vector3.UnitZ;
    public Vector3 Up = Vector3.UnitY;
    public Vector3 Right = Vector3.UnitX;

    public Matrix4 GetViewMatrix()
    {
        return Matrix4.LookAt(
            Position,
            Position + Front,
            Up);
    }

    public void UpdateVectors()
    {
        Vector3 front;

        front.X = MathF.Cos(MathHelper.DegreesToRadians(Yaw)) *
                  MathF.Cos(MathHelper.DegreesToRadians(Pitch));

        front.Y = MathF.Sin(MathHelper.DegreesToRadians(Pitch));

        front.Z = MathF.Sin(MathHelper.DegreesToRadians(Yaw)) *
                  MathF.Cos(MathHelper.DegreesToRadians(Pitch));

        Front = Vector3.Normalize(front);

        Right = Vector3.Normalize(
            Vector3.Cross(Front, Vector3.UnitY));

        Up = Vector3.Normalize(
            Vector3.Cross(Right, Front));
    }
}