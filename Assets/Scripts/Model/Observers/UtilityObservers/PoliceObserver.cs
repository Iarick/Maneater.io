using System;
using System.Collections.Generic;
using UnityEngine;

public class PoliceObserver : ICreatureObserver
{
    // Список зарегистрированных спавнеров
    private List<Spawner> registeredSpawners = new List<Spawner>();

    // Время последнего срабатывания HandleEnemyDetected
    private float triggerTime = 10.0f;
    private float lastTriggerTime;

    // Последняя обнаруженная позиция
    private Vector3 _lastDetectedPosition;

    // Закрытый конструктор для предотвращения создания экземпляров извне
    private PoliceObserver() { }
    public void RegisterSpawner(Spawner spawner)
    {
        registeredSpawners.Add(spawner);
    }

    // Свойство для доступа к экземпляру синглтона
    public static PoliceObserver Instance
    {
        get
        {
            if (Controller.Instance.policeObserver == null)
            {
                InitializePoliceObserver();
            }
            return Controller.Instance.policeObserver;
        }
    }

    public static void InitializePoliceObserver()
    {
        Controller.Instance.policeObserver = new PoliceObserver();
    }

    // Свойство для доступа к последней обнаруженной позиции
    public Vector3 LastDetectedPosition
    {
        get { return _lastDetectedPosition; }
    }

    public void AddObserverTo(Creature creature)
    {
        creature.OnEnemyDetected += HandleEnemyDetected;
    }

    private void HandleEnemyDetected(List<Creature> enemies)
    {
        // Устанавливаем позицию последнего обнаруженного существа
        _lastDetectedPosition = enemies[0].transform.position;
        if (Time.time - lastTriggerTime > triggerTime)
        {
            // Выбираем случайный спавнер из списка зарегистрированных
            Spawner randomSpawner = registeredSpawners[UnityEngine.Random.Range(0, registeredSpawners.Count)];

            // Запускаем спавнер
            randomSpawner.SpawnCreature();

            // Обновляем время последнего срабатывания
            lastTriggerTime = Time.time;
        }
    }
}