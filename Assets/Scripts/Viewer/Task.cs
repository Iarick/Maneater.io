using TMPro;
using UnityEngine;

public class Task : MonoBehaviour
{
    public string description;
    [SerializeField] private TMP_Text text;
    void Start()
    {
        text.text = description;
    }
    public void CompleteTask()
    {
        description = "<s>" + description + "</s>";
        text.text = description;
    }
}
