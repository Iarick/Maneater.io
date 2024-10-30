using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.Collections;
using UnityEngine;
[Serializable]
public class GameSaver
{
    private List<(float, float, float)> allCreaturesPositions = new List<(float, float, float)>();
    private List<int> allCreaturesFactorieIds = new List<int>();
    private List<float> allCreaturesHPs = new List<float>();
    private List<int> observersIds = new List<int>();
    private int points;
    private int creatures;
    private int victims;
    private int autoClickerCost;
    private int autoClickerPrice;
    private float clickInterval;
    private bool autoClickerActive;
    private List<int> spawnersCount = new List<int>();
    private List<int> spawnersIds = new List<int>();
    private List<(float, float, float)> spawnersPositions = new List<(float, float, float)>();
    [NonSerialized] private Presenter presenter;
    public GameSaver(Presenter presenter)
    {
        this.presenter = presenter;
        //StartCoroutine(SaveGameCoroutine());
    }
    public void SaveObserverIds(int id)
    {
        observersIds.Add(id);
    }
    public bool LoadGame()
    {
        if (System.IO.File.Exists(Application.persistentDataPath + "/GameData.dat"))
        {
            GameSaver player = DeserializePlayerData(Application.persistentDataPath + "/GameData.dat");
            Controller.Instance.creatures = player.creatures;
            Controller.Instance.victims = player.victims;
            Controller.Instance.points = player.points;
            var buttons = MonoBehaviour.FindObjectsOfType<SetObserverButton>();

            foreach (var id in player.observersIds)
            {
                var result = presenter.SetObserver(id);
                if (result)
                {
                    foreach (var button in buttons)
                    {
                        if (button.observerId == id)
                        {
                            button.makeButtonActive();
                        }
                    }
                }
            }
            if(player.autoClickerActive)
            {
                var autoClicker = MonoBehaviour.FindObjectOfType<AutoClicker>();
                autoClicker.autoClickerCost = player.autoClickerCost;
                autoClicker.autoClickerPrice = player.autoClickerPrice;
                autoClicker.clickInterval = player.clickInterval;
                autoClicker.StartClicking();
            }

            for (int i = 0; i < player.allCreaturesPositions.Count; i++)
            {
                Vector3 position = new Vector3(player.allCreaturesPositions[i].Item1, player.allCreaturesPositions[i].Item2, player.allCreaturesPositions[i].Item3);
                var creature = Controller.Instance.CreateCreature(player.allCreaturesFactorieIds[i], position, Quaternion.identity, Vector3.one);
                creature.health = player.allCreaturesHPs[i];
            }

            var spawners = MonoBehaviour.FindObjectsOfType<Spawner>();
            int ind = 0;

            //Debug.Log(player.spawnersPositions.Count);
            foreach (var pos in player.spawnersPositions)
            {
                foreach (var spawner in spawners)
                {
                    float epsilon = 0.005f; // Небольшая погрешность

                    if (Mathf.Abs(spawner.transform.position.x - pos.Item1) < epsilon &&
                        Mathf.Abs(spawner.transform.position.y - pos.Item2) < epsilon &&
                        Mathf.Abs(spawner.transform.position.z - pos.Item3) < epsilon)
                        {
                            spawner.spawnedCount = player.spawnersCount[ind];
                            spawner.id = player.spawnersIds[ind];
                            //Debug.Log(player.spawnersCount[ind]);
                            //Debug.Log(pos);
                        }
                }
                ind++;
            }

            return true;
        }
        else
        {
            return false;
        }
    }


    public void SaveGame()
    {
        List<Creature> allCreatures = Controller.Instance.spatialHashGrid.GetAllCreatures();
        foreach (Creature creature in allCreatures)
        {
            allCreaturesPositions.Add((creature.transform.position.x, creature.transform.position.y, creature.transform.position.z));
            allCreaturesFactorieIds.Add(creature.factoryId);
            allCreaturesHPs.Add(creature.health);
        }  
        creatures = Controller.Instance.creatures;
        victims = Controller.Instance.victims;

        points = Controller.Instance.points;
        int points_cost = 10;
        // Прибавляем очки за обсерверы, в 2 раза больше за каждый обсервер
        for(int i = 0; i < observersIds.Count; i++)
        {
            points += points_cost;
            points_cost *= 2;
        }
        AutoClicker autoClicker = MonoBehaviour.FindObjectOfType<AutoClicker>();
        if (autoClicker != null)
        {
            autoClickerCost = autoClicker.autoClickerCost;
            autoClickerPrice = autoClicker.autoClickerPrice;
            clickInterval = autoClicker.clickInterval;
            autoClickerActive = autoClicker.autoClicking;
            //points += 5;
        }
        var spawners = MonoBehaviour.FindObjectsOfType<Spawner>();
        foreach (var spawner in spawners)
        {
            spawnersCount.Add(spawner.spawnedCount);
            spawnersIds.Add(spawner.id);
            var touple = (spawner.transform.position.x, spawner.transform.position.y, spawner.transform.position.z);
            spawnersPositions.Add(touple);
        }
        //Debug.Log(spawnersPositions.Count);
        //points -= (int)Math.Floor(Math.Log10(Math.Abs(victims)) + 1)*10;
        //points -= (int)Math.Floor(Math.Log10(Math.Abs(creatures)))*10;
        SerializePlayerData(this, Application.persistentDataPath + "/GameData.dat");
    }
    private void SerializePlayerData(GameSaver player, string filePath)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
        {
            formatter.Serialize(fileStream, player);
        }
    }

    // Method for deserializing the PlayerData object from a file
    private GameSaver DeserializePlayerData(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            throw new ArgumentException("File path cannot be null or empty", nameof(filePath));
        }

        try
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                if (fileStream == null)
                {
                    throw new FileNotFoundException("File not found", filePath);
                }

                BinaryFormatter formatter = new BinaryFormatter();
                return (GameSaver)formatter.Deserialize(fileStream);
            }
        }
        catch (IOException ex)
        {
            throw new IOException("Error reading from file", ex);
        }
        catch (SerializationException ex)
        {
            throw new SerializationException("Error deserializing object", ex);
        }
    }

    internal static void DeleteSave()
    {
        try
        {
            if (File.Exists(Application.persistentDataPath + "/GameData.dat"))
            {
                File.Delete(Application.persistentDataPath + "/GameData.dat");
            }
        }
        catch (IOException e)
        {
            Debug.LogError("Error deleting save file: " + e.Message);
        }
        catch (UnauthorizedAccessException e)
        {
            Debug.LogError("Access denied when deleting save file: " + e.Message);
        }
    }
}
