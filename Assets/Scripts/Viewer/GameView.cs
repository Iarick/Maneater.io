using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameView : MonoBehaviour
{
    public float panSpeed = 20f;
    public Vector2 panLimit;
    public Presenter presenter{get; private set;}
    public Creature prefab;
    public TMP_Text GenPointsText;
    public TMP_Text CreaturesText;
    public List<Task> tasksInGameView;
    public GameObject EndGameWindow;
    public TMP_Text EndGameText;
    private bool isAutoSave = true;

    void Awake()
    {
        // Находим основную камеру
        Camera mainCamera = Camera.main;
        // Добавляем компонент CameraController к камере
        mainCamera.gameObject.AddComponent<CameraController>();
        mainCamera.gameObject.GetComponent<CameraController>().setPan(panSpeed, panLimit);

        presenter = new Presenter(this);
    }
    void Start()
    {
        //StartCoroutine(AutoSave());
    }
    void OnDisable()
    {
        isAutoSave = false;
    }
    IEnumerator<YieldInstruction> AutoSave()
    {
        while (isAutoSave)
        {
            yield return new WaitForSeconds(600); // Сохранять каждые 600 секунд
            presenter.gameSaver.SaveGame();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    void OnApplicationPause()
    {
        presenter.gameSaver.SaveGame();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 clickPosition = Input.mousePosition;
            presenter.HandleActivation(clickPosition);
        }
    }

    public void SetGenPointsText(int genPoints)
    {
        GenPointsText.text = genPoints.ToString();
    }
    public void SetCreaturesText(int creatures)
    {
        CreaturesText.text = creatures.ToString();
    }
    /*public void OnApplicationFocus()
    {
        presenter.gameSaver.SaveGame();
    }*/
    public void GameEnd(bool win)
    {
        EndGameWindow.SetActive(true);
        Time.timeScale = 0.0f;
        if(win)
        {
            EndGameText.text = "You win! Enemies are now getting stronger";
        }
        else
        {
            EndGameText.text = "You lose!";
        }
    }
}