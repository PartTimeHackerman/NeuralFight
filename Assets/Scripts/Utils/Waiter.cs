using System;
using System.Collections;
using UnityEngine;

public class Waiter
{
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