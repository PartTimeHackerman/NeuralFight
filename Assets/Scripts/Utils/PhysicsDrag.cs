using UnityEngine;

public class PhysicsDrag : MonoBehaviour
{
    public float drag;

    private Rigidbody Body;

    private void Start()
    {
        Body = GetComponent<Rigidbody>();

        Body.drag = 0f;
    }

    private void FixedUpdate()
    {
        Body.velocity *= Mathf.Clamp01(1f - drag * Time.fixedDeltaTime);
    }
}