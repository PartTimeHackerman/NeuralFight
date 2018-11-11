
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    private static GameTimer instance;
    
    public float Elapsed = 0f;
    
    public static GameTimer get()
    {
        if (instance == null)
        {
            GameObject go = new GameObject("GameTimer");
            instance = go.AddComponent<GameTimer>();
        }

        return instance;
    }
    
    void Update()
    {
        Elapsed += Time.deltaTime;
    }
}