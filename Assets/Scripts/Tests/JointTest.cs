using UnityEngine;

public class JointTest : MonoBehaviour
{
    public float sum;

    public Vector3 up;

    private void Start()
    {
    }

    private void Update()
    {
        up = transform.up;
        sum = up.x + up.z;
    }
}