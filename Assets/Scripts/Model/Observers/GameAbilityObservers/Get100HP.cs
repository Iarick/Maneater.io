public class Get100HP : ICreatureObserver
{
    public void AddObserverTo(Creature creature)
    {
        creature.health += 100;
    }
}
