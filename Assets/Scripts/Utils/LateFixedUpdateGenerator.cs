using UnityEngine;

public class LateFixedUpdateGenerator : MonoBehaviour
{
    int currentFrame = -1;
    float lateFixedTime = 0;

    void Start()
    {
        lateFixedTime = Time.fixedTime;
    }

    /*void FixedUpdate()
    {
        // catchup if FixedUpdate is called several times a frame
        if (currentFrame == Time.frameCount)
        {
            InvokeLateUpdate();
        }
        currentFrame = Time.frameCount;
    }*/

    void LateUpdate()
    {
        if (Time.fixedTime > lateFixedTime || Time.fixedTime == 0f)
            InvokeLateUpdate();
        lateFixedTime = Time.fixedTime;
    }
    void InvokeLateUpdate()
    {
        lateFixedTime += Time.fixedDeltaTime;
        SendMessage("LateFixedUpdate", SendMessageOptions.DontRequireReceiver);
    }
}
