using OpenTK.Mathematics;
using Engine.ECS;
namespace Engine.Core;

public class Transform : Component
{
    public Vector3 Position = Vector3.Zero;

    public Vector3 Rotation = Vector3.Zero;

    public Vector3 Scale = Vector3.One;

    public Matrix4 GetModelMatrix()
    {
        Matrix4 translation =
            Matrix4.CreateTranslation(Position);

        Matrix4 rotationX =
            Matrix4.CreateRotationX(MathHelper.DegreesToRadians(Rotation.X));

        Matrix4 rotationY =
            Matrix4.CreateRotationY(MathHelper.DegreesToRadians(Rotation.Y));

        Matrix4 rotationZ =
            Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(Rotation.Z));

        Matrix4 scale =
            Matrix4.CreateScale(Scale);

        return translation * rotationZ * rotationY * rotationX * scale;
    }
}