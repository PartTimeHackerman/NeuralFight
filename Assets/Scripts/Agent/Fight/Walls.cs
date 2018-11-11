using UnityEngine;

public class Walls : MonoBehaviour
{
    public WallMover left;
    public WallMover right;

    void Start()
    {
        
    }

    public float getCurrentPos()
    {
        return right.transform.position.x;
    }

    public void ResetWalls()
    {
        left.ResetWall();
        right.ResetWall();
    }
}