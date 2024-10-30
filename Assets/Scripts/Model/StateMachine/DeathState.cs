public class DeathState : ICreatureState
{
    public void EnterState(Creature creature)
    {
        creature.stateName = "Death";
    }

    public void Update(Creature creature)
    {
        
    }
}
