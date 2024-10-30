using UnityEngine;
using System.Collections;

public abstract class Spawner : MonoBehaviour
{
    [SerializeField] protected Creature creaturePrefab;
    [SerializeField] protected float spawnInterval;
    [SerializeField] protected int spawnCount;
    [SerializeField] public int id;

    public int spawnedCount;
    protected ICreatureFactory factory;

    public abstract void InitializeFactory(ICreatureFactory factory, Creature creature);
    public abstract void StartSpawning();
    public abstract void SpawnCreature();

    protected IEnumerator SpawnRoutine()
    {
        while (spawnedCount < spawnCount)
        {
            SpawnCreature();
            spawnedCount++;
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
