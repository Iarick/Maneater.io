public class ReturnDamage : ICreatureObserver
{
    private Creature thisCreature;
    public void AddObserverTo(Creature creature)
    {
        thisCreature = creature;
        creature.OnAttack += OnAttakedHandler;
    }
    private void OnAttakedHandler(float damage,Creature creature)
    {
        if(damage > 1)
        {
            creature.DecreaseHealth(damage*0.5f, thisCreature);
        }
    }
}
