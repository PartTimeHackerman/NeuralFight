using System.Collections.Generic;
using UnityEngine;

public class CenterOfMassDebug : MonoBehaviour
{
    private BodyParts parts;
    private PhysicsUtils physics;
    private GameObject sphere;
    public List<Rigidbody> rigids;

    private void Start()
    {
        parts = GetComponent<BodyParts>();
        physics = PhysicsUtils.get();
        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Destroy(sphere.GetComponent<SphereCollider>());
        sphere.transform.localScale = new Vector3(.1f, .1f, .1f);
        sphere.GetComponent<MeshRenderer>().material.color = Color.red;
        rigids = parts.getRigids();
    }

    private void FixedUpdate()
    {
        Vector3 COM = physics.getCenterOfMass(rigids);
        COM.x = 3;
        sphere.transform.position = COM;
    }
}