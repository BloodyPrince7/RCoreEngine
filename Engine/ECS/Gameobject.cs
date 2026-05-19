namespace Engine.ECS;
public class GameObject
{
    public string Name;
    public List<Component> Components = new();
    public GameObject(string name)
    {
        Name = name;
    }
    public T AddComponent<T>() where T : Component, new()
    {
        T component = new T { GameObject = this };
        Components.Add(component);
        return component;
    }
    public T GetComponent<T>() where T : Component
    {
        foreach (var component in Components)
        {
            if (component is T t)
                return t;
        }
        return null;
    }
    public void Start()
    {
        foreach (var component in Components)
        {
            component.Start();
        }
    }
    public void Update(float deltaTime)
    {
        foreach (var component in Components)
        {
            component.Update(deltaTime);
        }
    }
}