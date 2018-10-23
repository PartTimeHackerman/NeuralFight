using UnityEngine;

public class RbCOM : MonoBehaviour
{
    public Vector3 com;
    public Rigidbody rb;
    private GameObject sphere;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.centerOfMass = GetComponent<BoxCollider>().center;
        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Destroy(sphere.GetComponent<SphereCollider>());
        sphere.transform.localScale = new Vector3(.05f, .05f, .05f);
        sphere.GetComponent<MeshRenderer>().material.color = Color.blue;
        
    }

    private void FixedUpdate()
    {
        com = rb.centerOfMass;
        sphere.transform.position = transform.TransformPoint(com);
    }
}