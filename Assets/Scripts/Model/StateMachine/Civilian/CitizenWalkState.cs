using System;
using UnityEngine;
using System.Collections.Generic;

public class CitizenWalkState : ICreatureState
{
    private float timer = 0;
    private float distance = 200f;
    private Action<List<Creature>> enemiesDelegate;
    private Creature managedCreature;

    public void EnterState(Creature creature)
    {
        managedCreature = creature;
        // Выбираем случайное направление для движения
        Vector3 randomDirection = creature.transform.position + new Vector3(distance*UnityEngine.Random.Range(-1f, 1f), 0, distance*UnityEngine.Random.Range(-1f, 1f));
        creature.Move(randomDirection);

        // Добавляем делегат к событию OnEnemyAttacked
        enemiesDelegate = (List<Creature> e) =>
        {
            managedCreature.OnEnemyDetected -= enemiesDelegate;
            managedCreature.SetState(new FleeState(e));
        };
        creature.OnEnemyDetected += enemiesDelegate;
    }

    public void Update(Creature creature)
    {
        timer += Time.deltaTime;
        if (timer >= 20)
        {
            // Выбираем новое случайное направление каждые 20 секунд
            Vector3 randomDirection = creature.transform.position + new Vector3(distance*UnityEngine.Random.Range(-1f, 1f), 0, distance*UnityEngine.Random.Range(-1f, 1f));
            creature.Move(randomDirection);
            timer = 0;
        }
    }
}
