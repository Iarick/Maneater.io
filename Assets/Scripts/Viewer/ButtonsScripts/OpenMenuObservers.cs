using UnityEngine;
using UnityEngine.UI;

public class OpenMenuObservers : MonoBehaviour
{
    private bool isMenuOpen = false; // Флаг состояния меню (открыто/закрыто)

    public GameObject menuCanvas; // Ссылка на объект канваса меню

    void Start()
    {
        menuCanvas.SetActive(false); // Начальное состояние - меню закрыто
        GetComponent<Button>().onClick.AddListener(OpenMenu);
    }

    void OpenMenu()
    {
        if(Time.timeScale != 0)
        {
            isMenuOpen = true;
            menuCanvas.SetActive(isMenuOpen);
        }
        else if(isMenuOpen)
        {
            isMenuOpen = false;
            menuCanvas.SetActive(isMenuOpen);
        }
        else
        {
            return;
        }
        if(isMenuOpen)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
