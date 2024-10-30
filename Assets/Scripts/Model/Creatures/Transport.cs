using System.Collections;
using UnityEngine.AI;
using UnityEngine;

public class Transport : Creature
{
    private NavMeshAgent agent;
    //[SerializeField] private Animator animator;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        SetState(new TransportState());
    }
    protected override void _AttackEnemy(Creature enemy){}

    protected override void _DecreaseHealth(float damage)
    {
        health -= damage; // Уменьшаем здоровье на величину урона
    }

    protected override void _DestroyEnemy(Creature enemy){}

    protected override void _Die(Creature corpus)
    {
        StartCoroutine(DestroyAfterDelay());
    }

    protected override void _Move(Vector3 destination)
    {
        if (agent != null && agent.isOnNavMesh)
        {
            agent.SetDestination(destination);
            agent.speed = speed;
        }
    }

    protected override void _Split(Creature original){}

    protected override void _Attacked(float damage, Creature attacker)
    {
        DecreaseHealth(damage, attacker);
    }
    private IEnumerator DestroyAfterDelay()
    {
        // Ожидаем небольшую задержку
        yield return new WaitForSeconds(1f);
        // Удаляем объект существа
        Destroy(gameObject);
    }
}
