using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierSpawner : Spawner
{
    private void Start()
    {
        InitializeFactory(factory, creaturePrefab);
        if(spawnCount>=6)
        {
            spawnCount +=  PlayerPrefs.GetInt("points", 0)/10;
        }
    }

    public override void InitializeFactory(ICreatureFactory factory, Creature creature)
    {
        factory = new CreatureType1Factory(creaturePrefab);
        Controller.Instance.AddCreatureFactory(id, factory);
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
