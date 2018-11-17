using UnityEngine;

public class TargetDist : MonoBehaviour
{
    public Transform from;
    public float distance;

    private void FixedUpdate()
    {
        distance = Vector3.Distance(from.position, transform.position);
    }
}