public class GetEghtenHealth : ICreatureObserver
{
    public void AddObserverTo(Creature creature)
    {
        creature.health += 80;
    }
}
