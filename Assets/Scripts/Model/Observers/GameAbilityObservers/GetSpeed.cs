public class GetSpeed : ICreatureObserver
{
    public void AddObserverTo(Creature creature)
    {
        creature.speed += 2;
    }
}
