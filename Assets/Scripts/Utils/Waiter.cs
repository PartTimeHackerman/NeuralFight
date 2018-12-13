using System;
using System.Collections;
using UnityEngine;

public class Waiter : MonoBehaviour
{

    private static Waiter waiter;
    
    public static Waiter Get()
    {
        if (waiter == null)
        {
            GameObject go = new GameObject("Waiter");
            DontDestroyOnLoad(go);
            waiter = go.AddComponent<Waiter>();
        }

        return waiter;
    }

    
    public void WaitForFramesC(int frames, Action firstAction, Action secondAction)
    {
        StartCoroutine(WaitForFrames(frames, firstAction, secondAction));
    }
    
    public void WaitForSecondsC(float seconds, Action firstAction, Action secondAction)
    {
        StartCoroutine(WaitForSeconds(seconds, firstAction, secondAction));
    }
    
    public static IEnumerator WaitForFrames(int frames, Action firstAction, Action secondAction)
    {
        firstAction();
        for (int i = frames - 1; i >= 0; i--)
        {
            yield return new WaitForFixedUpdate();
        }

        secondAction();
    }

    public static IEnumerator WaitForSeconds(float seconds, Action firstAction, Action secondAction)
    {
        firstAction();
        yield return new WaitForSeconds(seconds);
        secondAction();
    }
}