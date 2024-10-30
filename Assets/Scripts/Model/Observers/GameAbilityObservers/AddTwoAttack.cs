public class AddTwoAttack : ICreatureObserver
{
    public void AddObserverTo(Creature creature)
    {
        creature.attackPower += 2;
    }
}
