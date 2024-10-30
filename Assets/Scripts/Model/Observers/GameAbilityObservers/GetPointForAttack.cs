public class GetPointForAttack : ICreatureObserver
{
    public void AddObserverTo(Creature creature)
    {
        creature.OnEnemyAttacked += OnAttackEventHandler;
    }
    private void OnAttackEventHandler(Creature creature)
    {
        Controller.Instance.points += 1;
    }
}
