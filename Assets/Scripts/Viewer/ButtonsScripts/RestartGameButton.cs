using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RestartSceneButton : MonoBehaviour
{
    private void Start()
    {
        // Назначьте этот метод на событие нажатия кнопки
        GetComponent<Button>().onClick.AddListener(RestartScene);
    }

    private void RestartScene()
    {
        Time.timeScale = 1;
        // Удаляем файл сохранения
        GameSaver.DeleteSave();
        // Получите текущую сцену и перезагрузите ее
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
