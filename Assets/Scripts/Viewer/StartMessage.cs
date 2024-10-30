using System.Collections;
using System.Threading;
using UnityEngine;

public class StartMessage : MonoBehaviour
{
    public GameObject uiObject;
    // Start is called before the first frame update
    void Start()
    {
        if(Controller.Instance.creatures == 1)
        {
            StartCoroutine(DelayedAction());
            uiObject.SetActive(true);
        }
    }

    IEnumerator DelayedAction()
    {
        // Подождать заданное время
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Controller.Instance.creatures != 1)
        {
            uiObject.SetActive(false);
            Time.timeScale = 1;
            Destroy(gameObject);
        }
    }
}
