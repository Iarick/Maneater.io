using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Citizen : Creature
{
    private NavMeshAgent agent;
    [SerializeField] private List<AudioClip> screams;
    [SerializeField] private Animator animator;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        SetState(new CitizenWalkState());
        PoliceObserver.Instance.AddObserverTo(this);
    }
    protected override void _AttackEnemy(Creature enemy){}

    protected override void _DecreaseHealth(float damage)
    {
        health -= damage; // Уменьшаем здоровье на величину урона
    }

    protected override void _DestroyEnemy(Creature enemy){}

    protected override void _Die(Creature corpus)
    {
        animator.SetBool("IsLife", false);
        animator.SetTrigger("Death");
        StartCoroutine(DestroyAfterDelay());
    }

    protected override void _Move(Vector3 destination)
    {
        if (agent != null && agent.isOnNavMesh)
        {
            agent.SetDestination(destination);
            agent.speed = speed;
        }
        if (speed == maxSpeed)
        {
            animator.SetTrigger("Running");
        }
        else
        {
            animator.SetTrigger("Walking");
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
    public void Evacuate()
    {
        StartCoroutine(DestroyAfterDelay());
    }

    public void Screaming()
    {
        GetComponent<AudioSource>().PlayOneShot(screams[UnityEngine.Random.Range(0, screams.Count)]);
    }
}
