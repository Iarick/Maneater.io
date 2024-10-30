using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class Creature : MonoBehaviour
{
    public string stateName = "default";
    public int id;
    public int factoryId;
    public float speed;
    public float health;
    public float maxHealth;
    public float maxSpeed;
    public float attackPower;
    public float detectionRange;
    public float maturityTime;
    public float attackRange;
    public float attackSpeed;
    public float size = 1.0f;
    public Vector2Int cellPosition;
    public bool isLife = true;

    // Определение событий
    public event Action<Vector3> OnMove;
    public event Action<Creature> OnEnemyAttacked;
    public event Action<Creature> OnEnemyDestroyed;
    public event Action<float, Creature> OnAttack;
    public event Action<float, Creature> OnHealthDecreased;
    public event Action<Creature> OnDeath;
    public event Action<Creature> OnSplit;
    public event Action<Creature> OnLife;
    public event Action<List<Creature>> OnEnemyDetected;
    public event Action OnDestroyed;


    private ICreatureState currentState;
    private float lastLifeTime;


    public void SetState(ICreatureState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    void Update()
    {
        if(currentState != null)
        {
            currentState.Update(this);
        }
        if(Time.time - lastLifeTime > 1f)
        {
            lastLifeTime = Time.time;
            OnLife?.Invoke(this);
        }
    }
    private void OnDestroy()
    {
        OnDestroyed?.Invoke();
    }

    protected abstract void _Move(Vector3 destination);
    protected abstract void _AttackEnemy(Creature enemy);
    protected abstract void _DestroyEnemy(Creature enemy);
    protected abstract void _DecreaseHealth(float damage);
    protected abstract void _Die(Creature corpus);
    protected abstract void _Attacked(float damage, Creature attacker);
    protected abstract void _Split(Creature original);

    // Методы для вызова событий
    public void Move(Vector3 destination)
    {
        OnMove?.Invoke(destination);
        _Move(destination); // Обращение к методу Move(Vector3 destination)
    }

    public void AttackEnemy(Creature enemy)
    {
        OnEnemyAttacked?.Invoke(enemy);
        _AttackEnemy(enemy); // Обращение к методу AttackEnemy(Creature enemy)
    }

    public void DestroyEnemy(Creature enemy)
    {
        OnEnemyDestroyed?.Invoke(enemy);
        _DestroyEnemy(enemy); // Обращение к методу DestroyEnemy(Creature enemy)
    }
    public void Attacked(float damage, Creature attacker)
    {
        OnAttack?.Invoke(damage, attacker);
        _Attacked(damage, attacker); // Обращение к методу Attacked(float damage, Creature attacker)
    }
    public void DecreaseHealth(float damage, Creature source)
    {
        OnHealthDecreased?.Invoke(damage, source);
        _DecreaseHealth(damage); // Обращение к методу DecreaseHealth()
        if (health <= 0 && isLife)
        {
            Die(this); // Если здоровье стало 0 или меньше, вызываем метод Died
            source.DestroyEnemy(this);
        }
    }

    protected void Die(Creature corpus)
    {
        isLife = false;
        OnDeath?.Invoke(corpus);
        _Die(corpus); // Обращение к методу Die()
    }
    public void Split(Creature original)
    {
        OnSplit?.Invoke(original);
        _Split(original); // Обращение к методу Split()
    }
    public void EnemyDetected(List<Creature> enemies)
    {
        OnEnemyDetected?.Invoke(enemies);
    }
}
