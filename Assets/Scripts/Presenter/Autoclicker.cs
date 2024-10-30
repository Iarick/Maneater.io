using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AutoClicker : MonoBehaviour
{
    public bool autoClicking = false;
    public float clickInterval = 4f; // Интервал между кликами (в секундах)
    public int autoClickerPrice = 5;
    public int autoClickerCost = -5;
    private float minClickInterval = 0.1f; // Минимальный интервал между кликами (в секундах)

    private IEnumerator AutoClickCoroutine()
    {
        while (true)
        {
            if (autoClicking)
            {
                // Вызываем метод случайного объекта
                if (CountCreatureObserver.creatures.Count > 0)
                {
                    int randomIndex = Random.Range(0, CountCreatureObserver.creatures.Count);
                    if(CountCreatureObserver.creatures[randomIndex] != null)
                    {
                        CountCreatureObserver.creatures[randomIndex].GetComponent<IActivableCreature>().ActivateCreature();
                    }
                }
            }

            yield return new WaitForSeconds(clickInterval);
        }
    }

    public void StartAutoClicker()
    {
        if(Controller.Instance.points >= autoClickerPrice)
        {
            autoClickerCost += autoClickerPrice;
            Controller.Instance.points -= autoClickerPrice;
            autoClickerPrice += Mathf.FloorToInt(autoClickerPrice * 0.2f);
            if (!autoClicking)
            {
                autoClicking = true;
                StartCoroutine(AutoClickCoroutine());
            }
            else
            {
                clickInterval = Mathf.Max(clickInterval - clickInterval * 0.1f, minClickInterval);
            }
        }
    }
    public void StartClicking()
    {
        autoClicking = true;
        StartCoroutine(AutoClickCoroutine());
    }
}
