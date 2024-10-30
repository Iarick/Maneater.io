public class AdvancedPointsForTime : ICreatureObserver
{
    private int currentTime = 0;
    public void AddObserverTo(Creature creature)
    {
        creature.OnLife += OnLifeEventHandler;
    }

    private void OnLifeEventHandler(Creature creature)
    {
        currentTime += 1;
        if (currentTime >= 180)
        {
            currentTime = 0;
            Controller.Instance.points += 1;
        }
    }
}
