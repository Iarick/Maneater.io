using System;
using System.Collections.Generic;
using UnityEngine;
public class SearchingState : ICreatureState
{
    private float timer = 0;
    private float distance = 200f;
    private Action<List<Creature>> enemiesDelegate;
    private Creature managedCreature;

    public void EnterState(Creature creature)
    {
        managedCreature = creature;
        Vector3 randomDirection = creature.transform.position + new Vector3(distance*UnityEngine.Random.Range(-1f, 1f), 0, distance*UnityEngine.Random.Range(-1f, 1f));
        creature.Move(randomDirection);

        enemiesDelegate = (List<Creature> e) =>
        {
            managedCreature.OnEnemyDetected -= enemiesDelegate;
            managedCreature.SetState(new FightingState(e));
        };
        creature.OnEnemyDetected += enemiesDelegate;
    }

    public void Update(Creature creature)
    {
        timer += Time.deltaTime;
        if (timer >= 10)
        {
            Vector3 randomDirection = creature.transform.position + new Vector3(distance*UnityEngine.Random.Range(-1f, 1f), 0, distance*UnityEngine.Random.Range(-1f, 1f));
            creature.Move(randomDirection);
            timer = 0;
        }
    }
}
