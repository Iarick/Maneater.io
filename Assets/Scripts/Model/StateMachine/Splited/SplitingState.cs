using System;
using UnityEngine;

public class SplittingState : ICreatureState
{
    private Type callingState; // Состояние, вызвавшее SplitState
    private float splitAngle; // Угол разделения
    private float splitDistance = 0.5f; // Расстояние разделения
    private float splitDuration = 0.2f; // Продолжительность разделения
    private float splitTimer; // Таймер для отсчета времени разделения
    private Vector3 startPosition; // Начальная позиция существа
    private Vector3 targetPosition; // Целевая позиция существа

    // Конструктор принимает вызвавшее состояние, по умолчанию None
    public SplittingState(ICreatureState callingState = null)
    {
        this.callingState = callingState?.GetType();;
        // Угол разделения зависит от времени
        splitAngle = Time.time % (2 * Mathf.PI);
        // Если вызвавшее состояние None, угол делаем противоположным
        if (callingState == null)
        {
            splitAngle += Mathf.PI;
        }
    }

    public void EnterState(Creature creature)
    {
        creature.stateName = "Spliting";
        // Запоминаем начальную позицию и определяем целевую позицию
        startPosition = creature.transform.position;
        Vector3 splitDirection = new Vector3(Mathf.Cos(splitAngle), 0, Mathf.Sin(splitAngle)).normalized;
        targetPosition = startPosition + splitDirection * splitDistance;

        // Устанавливаем таймер
        splitTimer = splitDuration;
        splitDistance *= creature.transform.localScale.x; // Увеличиваем расстояние разделения для разных размеров существ
    }

    public void Update(Creature creature)
    {
        // Обновляем таймер
        splitTimer -= Time.deltaTime;

        // Плавно перемещаем существо к целевой позиции
        creature.transform.position = Vector3.Lerp(startPosition, targetPosition, 1 - (splitTimer / splitDuration));

        // Проверяем, не истекло ли время разделения
        if (splitTimer <= 0)
        {
            // Переключаем состояние на вызвавшее или на Walk, если такого нет
            ICreatureState newState = (callingState != null) ? 
                (ICreatureState)Activator.CreateInstance(callingState) : 
                new RandomMovementState();
            creature.SetState(newState);
        }   
    }
}
