using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FightingState : ICreatureState
{
    private List<Creature> enemies;
    private Creature target;
    public FightingState(List<Creature> _enemies)
    {
        enemies = _enemies;
    }

    public void EnterState(Creature creature)
    {
        creature.OnEnemyDetected += EnemyDetectedHandler;
        creature.OnDeath += HandleDeath;
    }

    public void Update(Creature creature)
    {
        enemies?.RemoveAll(enemy => enemy == null);
        if (enemies != null && enemies.Count > 0)
        {
            target = GetClosestEnemy(creature, enemies);
            float distanceToTarget = Vector3.Distance(creature.transform.position, target.transform.position);

            if (distanceToTarget < creature.attackRange * 0.75f)
            {
                Vector3 directionAwayFromEnemy = (creature.transform.position - target.transform.position).normalized;
                Vector3 newPosition = creature.transform.position + directionAwayFromEnemy;
                creature.Move(newPosition);
            }
            else if (distanceToTarget <= creature.attackRange)
            {
                Vector3 directionToEnemy = (target.transform.position - creature.transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(directionToEnemy);
                creature.transform.rotation = Quaternion.Slerp(creature.transform.rotation, lookRotation, Time.deltaTime * creature.speed);
                creature.AttackEnemy(target);
            }
            else if (distanceToTarget <= creature.detectionRange)
            {
                creature.Move(target.transform.position);
            }
        }
        else
        {
            creature.OnDeath -= HandleDeath;
            creature.OnEnemyDetected -= EnemyDetectedHandler;
            creature.SetState(new MoveToPointState());
            return;
        }
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

    private void HandleDeath(Creature creature)
    {
        creature.OnDeath -= HandleDeath;
        creature.OnEnemyDetected -= EnemyDetectedHandler;
        creature.SetState(new DeathState());
    }
}