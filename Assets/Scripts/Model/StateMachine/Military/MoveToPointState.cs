using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPointState : ICreatureState
{
    private float timer = 0;
    private Action<List<Creature>> enemiesDelegate;
    private Creature managedCreature;
    private Vector3 lastDetectedPosition;

    public void EnterState(Creature creature)
    {
        managedCreature = creature;
        lastDetectedPosition = PoliceObserver.Instance.LastDetectedPosition;
        if(lastDetectedPosition == null || Vector3.Distance(creature.transform.position, lastDetectedPosition) < 10f)
        {
            managedCreature.SetState(new SearchingState());
            return;
        }
        creature.Move(lastDetectedPosition);

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
            // Проверяем, достигло ли существо последней обнаруженной позиции
            if (Vector3.Distance(creature.transform.position, lastDetectedPosition) < 10f)
            {
                // Если да, выбираем новую позицию и начинаем движение к ней
                lastDetectedPosition = PoliceObserver.Instance.LastDetectedPosition;
                creature.Move(lastDetectedPosition);
            }
            timer = 0;
        }
    }
}