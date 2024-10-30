public class SpawnForVictim : ICreatureObserver
{
    private Creature thisCreature;
    public void AddObserverTo(Creature creature)
    {
        thisCreature = creature;
        creature.OnEnemyDestroyed += OnVictimHandler;
    }

    private void OnVictimHandler(Creature victim)
    {
        thisCreature.Split(thisCreature);
    }
}

