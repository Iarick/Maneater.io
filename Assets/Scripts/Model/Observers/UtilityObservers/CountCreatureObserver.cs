using System.Collections.Generic;

public class CountCreatureObserver : ICreatureObserver
{
    static public List<Creature> creatures = new List<Creature>();
    public void AddObserverTo(Creature creature)
    {
        creature.OnSplit += OnSplitCreatureEventHandler;
        creature.OnDeath += OnDeathCreatureEventHandler;
    }

    private void OnSplitCreatureEventHandler(Creature creature)
    {
        creatures.Add(creature);
        Controller.Instance.creatures += 1;
    }
    private void OnDeathCreatureEventHandler(Creature creature)
    {
        creatures.Remove(creature);
        Controller.Instance.creatures -= 1;
    }
}
