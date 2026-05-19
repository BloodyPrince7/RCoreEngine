namespace Engine.ECS;
public class Scene
{
    public List<GameObject> GameObjects = new();
    public void Add(GameObject gameObject)
    {
        GameObjects.Add(gameObject);
    }
    public void Start()
    {
        foreach (var gameObject in GameObjects)
        {
            gameObject.Start();
        }
    }
    public void Update(float deltaTime)
    {
        foreach (var gameObject in GameObjects)
        {
            gameObject.Update(deltaTime);
        }
    }
}