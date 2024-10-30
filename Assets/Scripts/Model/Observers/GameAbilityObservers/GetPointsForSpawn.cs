public class GetPointsForSpawn : ICreatureObserver
{
    public void AddObserverTo(Creature creature)
    {
        creature.OnSplit += OnSpawnHandler;
        creature.health -= 200;
    }
    private void OnSpawnHandler(Creature creature)
    {
        Controller.Instance.points += 1;
    }
}
