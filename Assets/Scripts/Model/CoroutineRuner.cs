using System.Collections;
using UnityEngine;

public class CoroutineRunner
{
    private static CoroutineRunner instance;
    private MonoBehaviour coroutineHost;

    private CoroutineRunner()
    {
        GameObject coroutineObject = new GameObject("CoroutineRunner");
        coroutineHost = coroutineObject.AddComponent<CoroutineHost>();
    }

    public static CoroutineRunner Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new CoroutineRunner();
            }
            return instance;
        }
    }

    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return coroutineHost.StartCoroutine(routine);
    }

    public void StopCoroutine(Coroutine routine)
    {
        coroutineHost.StopCoroutine(routine);
    }

    private class CoroutineHost : MonoBehaviour { }
}