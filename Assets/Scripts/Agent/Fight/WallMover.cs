using UnityEngine;

public class WallMover : MonoBehaviour
{
    public float maxSeconds = 30;
    public float elapsedSeconds = 0f;
    public float startTime;
    public bool run;

    public float endPos = 1;
    private float startPos;
    private bool left;

    void Start()
    {
        startPos = transform.position.x;
        left = startPos < 0;
        endPos = left ? -endPos : endPos;
    }

    void FixedUpdate()
    {
        elapsedSeconds = GameTimer.get().Elapsed;

        if (elapsedSeconds <= startTime + maxSeconds && run)
        {
            Vector3 pos = transform.position;
            pos.x = Mathf.Lerp(startPos, endPos, (elapsedSeconds - startTime) / maxSeconds);
            transform.position = pos;
        }
    }

    public void StartMoving()
    {
        startTime = GameTimer.get().Elapsed;
        run = true;
    }

    public void ResetWall()
    {
        Vector3 pos = transform.position;
        pos.x = startPos;
        transform.position = pos;
    }

    public void StopMoving()
    {
        run = false;
    }
}