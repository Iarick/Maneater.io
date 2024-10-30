using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class  Controller
{
    // Переменная для хранения текущей цены обсервера
    public int observerPrice = 10;
    public Presenter presenter;
    public PoliceObserver policeObserver;

    // Переменная для хранения количества очков
    private int _points = 0;
    public int points
    {
        get
        { 
            return _points;
        }
        set
        {
            _points = value;
            presenter.SetGenPointsText(_points);
        }
    }
    private int _creatures = 1;
    private bool getTenCreatures = false;
    private bool getHundredCreatures = false;
    private bool getThousandCreatures = false;
    public int creatures
    {
        get
        {
            return _creatures;
        }
        set
        {
            _creatures = value;
            if(_creatures == 0)
            {
                presenter.CompleteTask(-1);
            }
            if (_creatures >= 10 && getTenCreatures == false)
            {
                points += 10;
                getTenCreatures = true;
                presenter.CompleteTask(0);
            }
            if (_creatures >= 100 && getHundredCreatures == false)
            {
                points += 10;
                getHundredCreatures = true;
                presenter.CompleteTask(1);
            }
            if (_creatures >= 1000 && getThousandCreatures == false)
            {
                points += 10;
                getThousandCreatures = true;
                presenter.CompleteTask(2);
            }
            presenter.SetCreaturesText(_creatures);
        }
    }
    private int _victims = 0;
    private bool getVictim = false;
    private bool getTenVictims = false;
    private bool getHundredVictims = false;
    private bool getThousandVictims = false;
    public int victims
    {
        get
        {
            return _victims;
        }
        set
        {
            _victims = value;
            if(_victims >= 1 && getVictim == false)
            {
                points += 10;
                getVictim = true;
                presenter.CompleteTask(3);
            }
            if (_victims >= 10 && getTenVictims == false)
            {
                points += 10;
                getTenVictims = true;
                presenter.CompleteTask(4);
            }
            if (_victims >= 100 && getHundredVictims == false)
            {
                points += 10;
                getHundredVictims = true;
                presenter.CompleteTask(5);
            }
            if (_victims >= 1000 && getThousandVictims == false)
            {
                points += 10;
                getThousandVictims = true;
                presenter.CompleteTask(6);
            }
        }
    }
    private static Controller instance;
    public static Controller Instance
    {
        get
        {
            if (instance == null)
            {
                InitializeController();
            }
            return instance;
        }
        set
        {
            instance = value;
        }
    }
    public SpatialHashGrid spatialHashGrid;

    // Словарь для хранения префабов существ
    private Dictionary<int, GameObject> creaturePrefabs = new Dictionary<int, GameObject>();

    // Словарь для хранения экземпляров фабрик
    private Dictionary<int, ICreatureFactory> creatureFactories = new Dictionary<int, ICreatureFactory>();

    // Словарь для хранения наблюдателей
    private Dictionary<int, List<ICreatureObserver>> creatureObservers = new Dictionary<int, List<ICreatureObserver>>();

    // Глобальные настройки
    public float GlobalSpeed { get; set; }

    private Controller()
    {
        // Инициализация глобальных настроек
        GlobalSpeed = 1.0f;
    }

    public static void InitializeController()
    {
        Instance = new Controller();
        Instance.spatialHashGrid = new SpatialHashGrid(10f);
        PoliceObserver.InitializePoliceObserver();
    }

    public bool CanAffordObserver()
    {
        // Сравниваем количество очков с ценой обсервера
        return points >= observerPrice;
    }

    // Метод для списания очков при установке обсервера
    public void DeductPoints(int amount)
    {
        // Уменьшаем количество очков на указанную сумму
        points -= amount;
        observerPrice *= 2;
    }

    // Метод для добавления префаба существа
    public void AddCreaturePrefab(int id, GameObject prefab)
    {
        creaturePrefabs[id] = prefab;
    }

    // Метод для добавления фабрики
    public void AddCreatureFactory(int id, ICreatureFactory factory)
    {
        creatureFactories[id] = factory;
        AddCreatureObserver(id, spatialHashGrid);
    }

    // Метод для добавления наблюдателя
    public void AddCreatureObserver(int id, ICreatureObserver observer)
    {
        if (!creatureObservers.ContainsKey(id))
        {
            creatureObservers[id] = new List<ICreatureObserver>();
        }
        creatureObservers[id].Add(observer);
    }

    // Специальная функция для создания существа
    public Creature CreateCreature(int factoryId, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        if (creatureFactories.ContainsKey(factoryId))
        {
            Creature creature = creatureFactories[factoryId].Create(position, rotation, scale);
            // Декорирование существа наблюдателями
            if (creatureObservers.ContainsKey(factoryId))
            {
                foreach (var observer in creatureObservers[factoryId])
                {
                    observer.AddObserverTo(creature);
                }
            }
            creature.factoryId = factoryId;
            return creature;
        }
        else
        {
            Debug.LogError("Фабрика с ID " + factoryId + " не найдена.");
            return null;
        }
    }
}
