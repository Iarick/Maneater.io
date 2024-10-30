public class EnemySpawner : Spawner
{
    public override void InitializeFactory(ICreatureFactory factory, Creature creature)
    {
        factory = new CreatureType1Factory(creaturePrefab);
        Controller.Instance.AddCreatureFactory(id, factory);
        StartSpawning();
    }

    public override void StartSpawning()
    {
        StartCoroutine(SpawnRoutine());
    }

    public override void SpawnCreature()
    {
        Controller.Instance.CreateCreature(id, transform.position, transform.rotation, transform.localScale);
    }
}