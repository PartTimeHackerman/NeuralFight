using UnityEngine;

public class WallMover : MonoBehaviour
{
    public float maxSeconds = 30;
    public float elapsedSeconds = 0f;

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

        if (elapsedSeconds <= maxSeconds)
        {
            Vector3 pos = transform.position;
            pos.x = Mathf.Lerp(startPos, endPos, elapsedSeconds / maxSeconds);
            transform.position = pos;
        }
    }

    public void ResetWall()
    {
        Vector3 pos = transform.position;
        pos.x = transform.position.x;
        transform.position = pos;
    }
}