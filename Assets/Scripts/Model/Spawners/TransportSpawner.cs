using UnityEngine;
using System.Collections.Generic;

public class TransportSpawner : Spawner
{
    [SerializeField] private List<Creature> creatures;
    [SerializeField] private List<int> creaturesFactoriesIds;
    private int spawnerActNumber = 0;
    private int currentCreatureIndex = 0;

    public override void InitializeFactory(ICreatureFactory factory, Creature creature)
    {
        // Добавляем фабрику существ в контроллер
        factory = new CreatureType1Factory(creaturePrefab);
        Controller.Instance.AddCreatureFactory(id, factory);
        // Добавляем все фабрики в контроллер
        for (int i = 0; i < creatures.Count; i++)
        {
            Controller.Instance.AddCreatureFactory(creaturesFactoriesIds[i], new CreatureType1Factory(creatures[i]));
        }
        PoliceObserver.Instance.RegisterSpawner(this);
    }

    public override void StartSpawning()
    {
        // Запускаем корутину спавна
        StartCoroutine(SpawnRoutine());
    }

    public override void SpawnCreature()
    {
        // Создаем существо с помощью контроллера
        Controller.Instance.CreateCreature(id, transform.position, transform.rotation, transform.localScale);
        spawnerActNumber++;

        if (spawnerActNumber >= 10)
        {
            spawnerActNumber = 0; // Обнуляем счетчик
            // Проверяем, не вышли ли мы за пределы списка
            if (currentCreatureIndex >= creatures.Count)
            {
                return;
            }

            // Создаем новую фабрику и обновляем id спавнера
            //factory = new CreatureType1Factory(creatures[currentCreatureIndex]);
            id = creaturesFactoriesIds[currentCreatureIndex]; // Обновляем id спавнера
            //InitializeFactory(factory, creatures[currentCreatureIndex]);
            currentCreatureIndex++;
        }
    }
}
