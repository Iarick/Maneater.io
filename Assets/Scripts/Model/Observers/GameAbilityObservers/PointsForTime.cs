public class PointsForTime : ICreatureObserver
{
    public void AddObserverTo(Creature creature)
    {
        var currentTime = 0;
        creature.OnLife += (Creature _) =>
        {
            currentTime += 1;
            if (currentTime >= 600)
            {
                currentTime = 0;
                Controller.Instance.points += 1;
            }
        };
    }
}
