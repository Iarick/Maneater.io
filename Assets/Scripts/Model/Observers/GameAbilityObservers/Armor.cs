public class Armor : ICreatureObserver
{
    private Creature thisCreature;
    public void AddObserverTo(Creature creature)
    {
        thisCreature = creature;
        thisCreature.OnAttack += OnAttakedHandler;
    }

    private void OnAttakedHandler(float damage,Creature creature)
    {
        if(damage > 3)
        {
            thisCreature.health += 3;
        }
    }
}
