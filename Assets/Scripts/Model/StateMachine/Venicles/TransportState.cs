using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TransportState : ICreatureState
{
    private float timer = 0;
    private Action<List<Creature>> enemiesDelegate;
    private Creature managedCreature;
    private Vector3 lastDetectedPosition;

    public void EnterState(Creature creature)
    {
        managedCreature = creature;
        lastDetectedPosition = PoliceObserver.Instance.LastDetectedPosition;
        creature.Move(lastDetectedPosition);

        enemiesDelegate = (List<Creature> e) =>
        {
            managedCreature.OnEnemyDetected -= enemiesDelegate;
            managedCreature.GetComponent<NavMeshAgent>().isStopped = true;
            // Активируем компонент Spawner на существе при обнаружении врага
            managedCreature.GetComponent<Spawner>().StartSpawning();
            if(managedCreature.GetComponent<AudioSource>() != null)
            {
                managedCreature.GetComponent<AudioSource>().Stop();
            }
            managedCreature.SetState(new DeathState());
        };
        creature.OnEnemyDetected += enemiesDelegate;
    }

    public void Update(Creature creature)
    {
        timer += Time.deltaTime;
        if (timer >= 10)
        {
            // Проверяем, достигло ли существо последней обнаруженной позиции
            if (Vector3.Distance(creature.transform.position, lastDetectedPosition) > 25f)
            {
                // Если нет, продолжаем движение к этой позиции
                creature.Move(lastDetectedPosition);
            }
            else
            {
                // Если да, выбираем новую позицию и начинаем движение к ней
                lastDetectedPosition = PoliceObserver.Instance.LastDetectedPosition;
                creature.Move(lastDetectedPosition);
            }
            timer = 0;
        }
    }
}