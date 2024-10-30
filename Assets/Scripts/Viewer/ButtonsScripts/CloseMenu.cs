using UnityEngine;
using UnityEngine.UI;

public class CloseMenu : MonoBehaviour
{
    public GameObject menuCanvas; // Ссылка на объект канваса меню

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(ClosePanel);
    }

    void ClosePanel()
    {
        menuCanvas.SetActive(false);
        Time.timeScale = 1.0f;
    }
}
