using System;
using System.Collections.Generic;
using UnityEngine;

public class Presenter
{
    private GameView view;
    private Controller model;
    // Список типов, реализующих интерфейс IObserver
    private List<Type> observerTypes;
    private float lastTime = Time.time;
    public GameSaver gameSaver;


    public Presenter(GameView view)
    {
        this.view = view;
        Controller.InitializeController();
        model = Controller.Instance;
        model.presenter = this;
        var factory = new CreatureType1Factory(view.prefab);
        model.AddCreatureFactory(view.prefab.id, factory);
        model.AddCreatureObserver(view.prefab.id, new CountCreatureObserver());
        model.AddCreatureObserver(view.prefab.id, new CountVictimObserver());
        //Ищем все спавнеры
        var spawners = MonoBehaviour.FindObjectsOfType<Spawner>();
        foreach (var spawner in spawners)
        {
            spawner.InitializeFactory(factory, view.prefab);
        }
        //var spawner = MonoBehaviour.FindObjectOfType<SpawnPlayer>();
        //spawner.InitializeFactory(factory, view.prefab);
        InitializeObserverTypes();
        gameSaver = new GameSaver(this);
        gameSaver.LoadGame();
        Controller.Instance.points += PlayerPrefs.GetInt("points", 0);
    }

    private void InitializeObserverTypes()
    {
        observerTypes = new List<Type>
        {
            typeof(LifeTimeSplitObserver),//0
            typeof(AddTwoAttack),//1
            typeof(AddFortenHealth),//2
            typeof(GetEghtenHealth),//3
            typeof(Get100HP),//4
            typeof(GetPointsForVictum),//5
            typeof(GetPointsForSpawn),//6
            typeof(AddAttackAndHealth),//7
            typeof(Armor),//8
            typeof(ReturnDamage),//9
            typeof(SpawnForVictim),//10
            typeof(GetAttackSpeed),//11
            typeof(GetSpeed),//12
            typeof(PointsForTime),//13
            typeof(AdvancedPointsForTime),//14
            typeof(GetPointForAttack)//15
        };
    }

    public void HandleActivation(Vector3 clickPosition)
    {
        if(Time.timeScale != 0 || Controller.Instance.creatures == 1)
        {
            /*if(Time.time - lastTime < 15f)
            {
                lastTime = Time.time;
                GameSaver.DeleteSave();
                gameSaver.SaveGame();
            }*/
            Ray ray = Camera.main.ScreenPointToRay(clickPosition);
            RaycastHit hit;

            // Увеличьте радиус сферы, чтобы охватить ближайшие объекты
            float sphereRadius = 1f; 

            if (Physics.SphereCast(ray, sphereRadius, out hit))
            {
                IActivableCreature activableCreature = hit.collider.GetComponent<IActivableCreature>();
                if (activableCreature != null)
                {
                    activableCreature.ActivateCreature();
                }
            }
        }
    }

    public bool SetObserver(int observerId, int factoryId = 0)
    {

        // Проверяем, хватает ли очков для установки обсервера, вызывая метод CanAffordObserver у контроллера
        if (model.CanAffordObserver())
        {
            // Находим тип по id в списке observerTypes
            Type observerType = observerTypes[observerId];
            // Сохраняем id наблюдателя
            gameSaver.SaveObserverIds(observerId);

            // Создаем экземпляр этого типа с помощью метода Activator.CreateInstance()
            ICreatureObserver observer = (ICreatureObserver)Activator.CreateInstance(observerType);

            // Вызываем метод AddCreatureObserver у контроллера, передавая ему id фабрики и наблюдатель
            model.AddCreatureObserver(factoryId, observer);

            // Вызываем метод DeductPoints у контроллера, передавая ему observerPrice
            model.DeductPoints(model.observerPrice);

            //Устанавливаем наблюдатель на существ которые уже в игре
            foreach(Creature c in CountCreatureObserver.creatures)
            {
                observer.AddObserverTo(c);
            }

            // Возвращаем true, означающее, что обсервер установлен
            return true;
        }
        else
        {
            // Здесь можно написать логику, которая будет выполняться, если очков не хватает
            Debug.Log("Not enough points");

            // Возвращаем false, означающее, что обсервер не установлен
            return false;
        }
    }
    public void SetGenPointsText(int points)
    {
        view.SetGenPointsText(points);
    }
    public void SetCreaturesText(int creatures)
    {
        view.SetCreaturesText(creatures);
    }
    public int GetObserverCost()
    {
        return model.observerPrice;
    }
    public void CompleteTask(int id)
    {
        if(id == 2 || id == 6)
        {
            PlayerPrefs.SetInt("points", PlayerPrefs.GetInt("points", 0) + 10);
        }
        if(id == 2)
        {
            view.GameEnd(true);
        }
        if(id == -1)
        {
            view.GameEnd(false);
            return;
        }
        view.tasksInGameView[id].CompleteTask();
    }
}