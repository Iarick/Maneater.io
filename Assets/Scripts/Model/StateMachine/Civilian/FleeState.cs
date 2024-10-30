using System;
using UnityEngine;
using System.Collections.Generic;

public class FleeState : ICreatureState
{
    private Action<List<Creature>> onEnemyDetectedDelegate;
    private List<Creature> enemies;
    private float safeDistance = 30f; // Задайте безопасное расстояние
    public FleeState(List<Creature> _enemies) { enemies = _enemies; } // Конструктор (список врагов в качестве аргумента>) { }

    public void EnterState(Creature creature)
    {
        creature.speed = creature.maxSpeed;
        if(creature is Citizen)
        {
            (creature as Citizen).Screaming();
        }
        // Добавляем делегат к событию OnEnemyDetected
        onEnemyDetectedDelegate = (List<Creature> _enemies) =>
        {
            // Получаем ближайшего врага
            enemies = _enemies;
            Creature nearestEnemy = GetNearestEnemy(creature, _enemies);
            if (nearestEnemy != null)
            {
                // Вычисляем направление от врага к существу
                Vector3 directionToEnemy = nearestEnemy.transform.position - creature.transform.position;
                // Вычисляем направление для бегства (противоположное направление к врагу)
                Vector3 fleeDirection = -directionToEnemy.normalized;
                creature.Move(fleeDirection);
            }
        };
        creature.OnEnemyDetected += onEnemyDetectedDelegate;
    }

    public void Update(Creature creature)
    {
        // Если существо на безопасном расстоянии от врагов, оно исчезает
        if (IsSafe(creature))
        {
            creature.OnEnemyDetected -= onEnemyDetectedDelegate;
            creature.SetState(new DeathState());
            (creature as Citizen)?.Evacuate();
        }
    }

    private Creature GetNearestEnemy(Creature creature, List<Creature> enemies)
    {
        Creature nearestEnemy = null;
        float minDistance = float.MaxValue;
        foreach (Creature enemy in enemies)
        {
            if (enemy == null) continue;
            float distance = Vector3.Distance(creature.transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = enemy;
            }
        }
        return nearestEnemy;
    }

    private bool IsSafe(Creature creature)
    {
        // Если существо на безопасном расстоянии от всех врагов, возвращаем true
        foreach (Creature enemy in enemies)
        {
            if (enemy == null) continue;
            if (Vector3.Distance(creature.transform.position, enemy.transform.position) <= safeDistance)
            {
                return false;
            }
        }
        return true;
    }
}
