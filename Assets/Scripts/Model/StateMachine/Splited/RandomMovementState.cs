using System.Collections.Generic;
using UnityEngine;

public class RandomMovementState : ICreatureState
{
    private Vector3 destination;
    private float changeDirectionTime = 5f; // Время в секундах, через которое объект будет менять направление
    private float timer;
    private Creature managedCreature;

    public void EnterState(Creature creature)
    {
        creature.stateName = "Move";
        managedCreature = creature;
        ChangeDirection(creature);
        creature.OnSplit += HandleSplit;
        creature.OnEnemyDetected += HandleEnemyDetected;
    }

    public void Update(Creature creature)
    {
        // Перемещаем объект к выбранной цели
        creature.Move(destination);

        // Увеличиваем таймер
        timer += Time.deltaTime;

        // Если прошло достаточно времени, меняем направление
        if (timer >= changeDirectionTime)
        {
            ChangeDirection(creature);
            timer = 0f; // Сбрасываем таймер
        }
    }
    private void HandleSplit(Creature creature)
    {
        // Отписываемся от события OnSplit
        creature.OnSplit -= HandleSplit;
        managedCreature.OnEnemyDetected -= HandleEnemyDetected;

        // Логика перехода в состояние разделения
        creature.SetState(new SplittingState(this));
    }
    private void HandleEnemyDetected(List<Creature> detectedEnemes)
    {
        managedCreature.OnEnemyDetected -= HandleEnemyDetected;
        managedCreature.OnSplit -= HandleSplit;
        managedCreature.SetState(new BattleState(detectedEnemes));
    }

    private void ChangeDirection(Creature creature)
    {
        // Генерируем случайную цель в пределах определенного радиуса
        float randomX = UnityEngine.Random.Range(-10f, 10f);
        float randomZ = UnityEngine.Random.Range(-10f, 10f);

        // Добавляем текущее положение существа к сгенерированной случайной позиции
        destination = creature.transform.position + new Vector3(randomX, 0, randomZ);
        creature.stateName = destination.ToString() + " Move";
    }
}