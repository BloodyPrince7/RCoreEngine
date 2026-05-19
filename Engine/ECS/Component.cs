using Engine.Core;

namespace Engine.ECS;
public class Component
{
   public GameObject GameObject ;
   public virtual void Start() { }
    public virtual void Update(float deltaTime ) { }
}