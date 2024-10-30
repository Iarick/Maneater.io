using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Linq;

public class BattleState : ICreatureState
{
    private List<Creature> enemies;
    private Creature target;
    public BattleState(List<Creature> _enemies)
    {
        enemies = _enemies;
    }

    public void EnterState(Creature creature)
    {
        creature.stateName = "Battle";
        // Подписываемся на события
        creature.OnEnemyDetected += EnemyDetectedHandler;
        creature.OnSplit += HandleSplit;
        creature.OnDeath += HandleDeath;
    }

    public void Update(Creature creature)
    {
        // Удаляем все уничтоженные объекты из списка
        enemies?.RemoveAll(enemy => enemy == null);
        // Если список врагов не пуст, выбираем ближайшего врага
        if (enemies != null && enemies.Count > 0)
        {
            target = GetClosestEnemy(creature, enemies);
            float distanceToTarget = Vector3.Distance(creature.transform.position, target.transform.position);

            // Если враг в пределах диапазона атаки, атакуем
            if (distanceToTarget <= creature.attackRange*target.size)
            {
                creature.AttackEnemy(target);
            }
            // Иначе, двигаемся к врагу
            else if (distanceToTarget <= creature.detectionRange)
            {
                creature.Move(target.transform.position);
                creature.stateName = target.transform.position.ToString();
            }
        }
        else
        {
            // Отписываемся от события
            creature.OnDeath -= HandleDeath;
            creature.OnSplit -= HandleSplit;
            creature.OnEnemyDetected -= EnemyDetectedHandler;
            // Переключаем состояние на Walk
            creature.SetState(new RandomMovementState());
            return;
        }
        // Если враг вышел из зоны обнаружения или был уничтожен, выбираем новую цель
        if (target == null || Vector3.Distance(creature.transform.position, target.transform.position) > creature.detectionRange)
        {
            target = GetClosestEnemy(creature, enemies);
            if (target != null)
            {
                creature.Move(target.transform.position);
            }
        }
    }

    private Creature GetClosestEnemy(Creature creature, List<Creature> enemies)
    {
        return enemies.OrderBy(e => Vector3.Distance(creature.transform.position, e.transform.position)).FirstOrDefault();
    }

    private void EnemyDetectedHandler(List<Creature> detectedEnemies)
    {
        enemies = detectedEnemies;
    }
    private void HandleSplit(Creature creature)
    {
        // Unsubscribe from the splitting event
        creature.OnDeath -= HandleDeath;
        creature.OnSplit -= HandleSplit;
        creature.OnEnemyDetected -= EnemyDetectedHandler;
        
        // Handle the logic for transitioning to the splitting state
        creature.SetState(new SplittingState());
    }

    private void HandleDeath(Creature creature)
    {
        // Unsubscribe from the death event
        creature.OnDeath -= HandleDeath;
        creature.OnSplit -= HandleSplit;
        creature.OnEnemyDetected -= EnemyDetectedHandler;
        
        // Handle the logic for transitioning to the death state
        creature.SetState(new DeathState());
    }
}
