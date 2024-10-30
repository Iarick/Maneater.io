public class AddFortenHealth : ICreatureObserver
{
    public void AddObserverTo(Creature creature)
    {
        creature.health += 40;
    }
}
