public class AddAttackAndHealth : ICreatureObserver
{
    public void AddObserverTo(Creature creature)
    {
        creature.attackPower += 2;
        creature.health += 40;
    }
}
