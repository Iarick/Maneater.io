public class SpawnPlayer : Spawner
{
    public override void SpawnCreature()
    {
        Controller.Instance.CreateCreature(id, transform.position, transform.rotation, transform.localScale);
    }

    public override void StartSpawning()
    {
        if (!System.IO.File.Exists("GameData.dat"))
        {
            StartCoroutine(SpawnRoutine());
        }
    }

    public override void InitializeFactory(ICreatureFactory factory, Creature creature)
    {
        this.factory = factory;
        creaturePrefab = creature;
        id = creaturePrefab.id;
        StartSpawning();
    }
}
