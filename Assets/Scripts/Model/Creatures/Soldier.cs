using UnityEngine.AI;
using UnityEngine;
using System.Collections;
using System;
public class Soldier : Creature
{
    private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem attackParticle; // Система частиц для атаки
    private float lastAttackTime;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        SetState(new MoveToPointState());
        PoliceObserver.Instance.AddObserverTo(this);
        //Рост силы врагов
        maxHealth +=  PlayerPrefs.GetInt("points", 0);
        health +=  PlayerPrefs.GetInt("points", 0);
        attackPower +=  PlayerPrefs.GetInt("points", 0)/2;
        attackSpeed +=  PlayerPrefs.GetInt("points", 0)/5;
        speed +=  Math.Max(5, PlayerPrefs.GetInt("points", 0)/20) ;
    }
    protected override void _Attacked(float damage, Creature attacker)
    {
        //animator.SetTrigger("GetHit");
        DecreaseHealth(damage, attacker); // Вызываем метод HealthDecreased с аргументом damage
    }

    protected override void _AttackEnemy(Creature enemy)
    {
        // Проверяем, находится ли враг в радиусе атаки
        if (Vector3.Distance(transform.position, enemy.transform.position) <= attackRange)
        {
            agent.SetDestination(transform.position);
            // Проверяем, прошло ли достаточно времени с последней атаки
            if (Time.time - lastAttackTime >= 10f / attackSpeed)
            {
                attackParticle.Play();
                animator.SetTrigger("Attack");
                // Обновляем время последней атаки
                lastAttackTime = Time.time;
                // Вызываем метод Attacked у врага
                enemy.Attacked(attackPower, this);
            }
        }
    }

    protected override void _DecreaseHealth(float damage)
    {
        health -= damage; // Уменьшаем здоровье на величину урона
    }

    protected override void _DestroyEnemy(Creature enemy)
    {
        
    }

    protected override void _Die(Creature corpus)
    {
        animator.SetTrigger("Die");
        animator.SetBool("IsLife", false);
        StartCoroutine(DestroyAfterDelay());
    }

    protected override void _Move(Vector3 destination)
    {
        agent.SetDestination(destination);
        animator.SetTrigger("Walk");
    }

    protected override void _Split(Creature original){}

    private IEnumerator DestroyAfterDelay()
    {
        // Ожидаем небольшую задержку
        yield return new WaitForSeconds(3f);
        // Удаляем объект существа
        Destroy(gameObject);
    }
}
