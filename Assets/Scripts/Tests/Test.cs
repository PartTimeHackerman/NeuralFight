using System;
using UnityEngine;

public class Test : MonoBehaviour
{
    private static Test instance;

    protected void Awake()
    {
        if (!enabled)
            return;
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        StartTest();
    }

    public virtual void StartTest()
    {
    }

    protected void Update()
    {
    }

    public static Test Instance()
    {
        if (!Exists())
            throw new Exception(
                "Test could not find the UnityMainThreadDispatcher object. Please ensure you have added the MainThreadExecutor Prefab to your scene.");
        return instance;
    }


    public static bool Exists()
    {
        return instance != null;
    }

    protected void OnDestroy()
    {
        instance = null;
    }
}