using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetObserverButton : MonoBehaviour
{
    // Ссылка на компонент Image кнопки
    public Image image;

    // Цвета для активной и неактивной кнопки
    public Color activeColor;
    public Color inactiveColor;
    public GameView gameView;
    public int observerId = 0;
    public bool active = false;
    public Button closeButton;
    public Button activateButton;
    public TMP_Text discriptionText;
    public TMP_Text cost;
    public GameObject panel;
    public string discription;
    public List<SetObserverButton> observers = new List<SetObserverButton>();
    private static bool hasOpened = false;

    // Метод, который вызывается при старте
    public void Start()
    {
        // Находим родительский объект GameView с помощью метода GetComponentInParent()
        //gameView = GetComponentInParent<GameView>();
        image.color = inactiveColor;
        activateButton.onClick.AddListener(OnButtonClick);
        GetComponent<Button>().onClick.AddListener(showPanel);
        closeButton.onClick.AddListener(hidePanel);
        discriptionText.text = discription;
        if (active)
        {
            image.color = activeColor;
        }
        else
        {
            image.color = inactiveColor;
        }
    }
    private void showPanel()
    {
        if (hasOpened) return;
        hasOpened = true;
        panel.SetActive(true);
        transform.SetAsLastSibling();
        //set panel to screen center
        panel.transform.position = new Vector3(Screen.width/2f, Screen.height/2f, 0);
        cost.text = "Cost: " + gameView.presenter.GetObserverCost().ToString();
    }
    private void hidePanel()
    {
        hasOpened = false;
        panel.SetActive(false);
    }
    // Метод, который вызывается при нажатии на кнопку
    private void OnButtonClick()
    {
        if(!active)
        {
            bool result = true;
            foreach (SetObserverButton observer in observers)
            {
                if (!observer.active)
                {
                    result = false;
                    break;
                }
            }
            if(result)
            {
                // Вызываем специальный метод у presenter, передавая ему id наблюдателя и получаем его возвращаемое значение
                active = gameView.presenter.SetObserver(observerId);
            }

            // В зависимости от результата, меняем цвет кнопки на активный или неактивный
            if (active)
            {
                image.color = activeColor;
            }
            else
            {
                image.color = inactiveColor;
            }

            // Здесь можно написать дополнительную логику, которая будет выполняться при нажатии на кнопку
            //Debug.Log("Button clicked");
            hidePanel();
        }
    }
    public void makeButtonActive()
    {
        foreach (SetObserverButton observer in observers)
        {
            if (!observer.active)
            {
                return;
            }
        }
        active = true;
    }
}