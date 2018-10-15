using UnityEngine;

public class PunchTerminator : MonoBehaviour
{

    public Rigidbody end;

    public bool isTerminated(Vector3 pointPos)
    {
        return Vector3.Distance(end.position, pointPos) < .2f;

    }

}