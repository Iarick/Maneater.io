
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetAutoClicker : MonoBehaviour
{
    [SerializeField] private AutoClicker autoClicker;
    [SerializeField] private TMP_Text discriptionText;
    private bool isActive = false;
    // Start is called before the first frame update
    void Start()
    {
        isActive =autoClicker.autoClicking;
        if(isActive)
        {
            discriptionText.text = "Upgrade autoclicker. Cost: " + autoClicker.autoClickerPrice.ToString() + " gen points. " + (1/autoClicker.clickInterval).ToString() + " clicks per second.";
        }
        else
        {
            discriptionText.text = "Buy autoclicker. Cost: " + autoClicker.autoClickerPrice.ToString() + " gen points. ";
        }
        if(autoClicker == null) { return; }
        GetComponent<Button>().onClick.AddListener(StartAutoClicker);
    }
    private void StartAutoClicker()
    {
        isActive = true;
        autoClicker.StartAutoClicker();
        discriptionText.text = "Upgrade autoclicker. Cost: " + autoClicker.autoClickerPrice.ToString() + " gen points. " + (1/autoClicker.clickInterval).ToString() + " clicks per second.";
    }
}
