public class GetAttackSpeed : ICreatureObserver
{
    public void AddObserverTo(Creature creature)
    {
        creature.attackSpeed += 3;
    }
}
