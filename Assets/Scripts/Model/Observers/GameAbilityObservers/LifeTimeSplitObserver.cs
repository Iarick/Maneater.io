public class LifeTimeSplitObserver : ICreatureObserver
{
    public float splitingProbability = 0.001f;
    public void AddObserverTo(Creature creature)
    {
        creature.OnLife += OnLifeEventHandler;
    }

    private void OnLifeEventHandler(Creature creature)
    {
        if (UnityEngine.Random.value <= splitingProbability)
        {
            creature.Split(creature);
        }
    }
}