using UnityEngine;

public class CenterOfMassDebug : MonoBehaviour
{
    private BodyParts parts;
    private PhysicsUtils physics;
    private GameObject sphere;

    private void Start()
    {
        parts = GetComponent<BodyParts>();
        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Destroy(sphere.GetComponent<SphereCollider>());
        sphere.transform.localScale = new Vector3(.1f, .1f, .1f);
        sphere.GetComponent<MeshRenderer>().material.color = Color.red;
    }

    private void Update()
    {
        sphere.transform.position = physics.getCenterOfMass(parts.getRigids());
    }
}