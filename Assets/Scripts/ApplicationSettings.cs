using System;
using UnityEngine;

public class ApplicationSettings : MonoBehaviour
{
    private static ApplicationSettings instance;

    public int FPS = 60;
    public float timeScale = 1;

    private void Awake()
    {
        if (instance == null) { 
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        Application.runInBackground = true;

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = FPS;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Application.targetFrameRate != FPS)
            Application.targetFrameRate = FPS;
        if (Time.timeScale != timeScale)
            Time.timeScale = timeScale;
    }

    public static ApplicationSettings Instance()
    {
        if (!Exists())
            throw new Exception(
                "UnityMainThreadDispatcher could not find the UnityMainThreadDispatcher object. Please ensure you have added the MainThreadExecutor Prefab to your scene.");
        return instance;
    }


    public static bool Exists()
    {
        return instance != null;
    }

    private void OnDestroy()
    {
        instance = null;
    }
}