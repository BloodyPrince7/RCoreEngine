using Engine.Graphics;
namespace Engine.ECS;
public class MeshRenderer : Component
{
    public Mesh Mesh;
    public Shader Shader;
    public void Render()
    {
        Shader.Use();
        Mesh.Draw();
    }
    
}