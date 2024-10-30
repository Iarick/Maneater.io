public class GetPointsForVictum : ICreatureObserver
{
    public void AddObserverTo(Creature creature)
    {
        creature.OnEnemyDestroyed += OnEnemyDestroyedHandler;
    }
    private void OnEnemyDestroyedHandler(Creature creature)
    {
        Controller.Instance.points += 1;
    }
}
