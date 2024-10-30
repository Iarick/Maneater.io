using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class WalkingSplittingCreature : Creature, IActivableCreature
{
    private NavMeshAgent agent;
    private Animator animator;
    private float lastAttackTime;
    private float lastSplitTime;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        animator = GetComponent<Animator>();
        SetState(new SplittingState());
    }
    protected override void _Move(Vector3 destination)
    {
        agent.SetDestination(destination);
        animator.SetTrigger("Walk");
    }
    protected override void _AttackEnemy(Creature enemy)
    {
        // Проверяем, находится ли враг в радиусе атаки
        if (Vector3.Distance(transform.position, enemy.transform.position) <= attackRange*enemy.size)
        {
            // Проверяем, прошло ли достаточно времени с последней атаки
            if (Time.time - lastAttackTime >= 10f / attackSpeed)
            {
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
    protected override void _Attacked(float damage, Creature attacker)
    {
        animator.SetTrigger("GetHit");
        DecreaseHealth(damage, attacker); // Вызываем метод HealthDecreased с аргументом damage
    }
    protected override void _Split(Creature original)
    {
        // Реализация логики разделения
        Controller.Instance.CreateCreature(id, original.transform.position, original.transform.rotation, original.transform.localScale);
    }

    protected override void _DestroyEnemy(Creature enemy)
    {
        // Пока оставляем пустым
    }


    protected override void _Die(Creature corpus)
    {
        animator.SetTrigger("Die");
        animator.SetBool("IsLife", false);
        StartCoroutine(DestroyAfterDelay());
    }


    public void ActivateCreature()
    {
        if(Time.time - lastSplitTime >= 0.2f)
        {
            Split(this);
            lastSplitTime = Time.time;
        }
    }

    private IEnumerator DestroyAfterDelay()
    {
        // Ожидаем небольшую задержку
        yield return new WaitForSeconds(3f);
        // Удаляем объект существа
        Destroy(gameObject);
    }
}

