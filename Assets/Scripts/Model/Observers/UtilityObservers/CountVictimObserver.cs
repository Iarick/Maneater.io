public class CountVictimObserver : ICreatureObserver
{
    public void AddObserverTo(Creature creature)
    {
        creature.OnEnemyDestroyed += OnEnemyDestroyedHandler;
    }
    public void OnEnemyDestroyedHandler(Creature creature)
    {
        Controller.Instance.victims += 1;
    }
}
