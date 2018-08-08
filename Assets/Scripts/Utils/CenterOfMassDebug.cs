using System.Collections.Generic;
using UnityEngine;

public class CenterOfMassDebug : MonoBehaviour
{
    private BodyParts parts;
    private PhysicsUtils physics;
    private GameObject sphere;
    private GameObject sphereV;
    private List<Rigidbody> rigids;

    public Vector3 COMReal;
    public Vector3 COMVelReal;
    public float COMVellMag;
    public Vector3 COMRotVelReal;

    public Vector3 COM;
    public Vector3 COMVel;
    public Vector3 COMRotVel;

    public Rigidbody root;
    public Vector3 COMRelative;
    public Vector3 COMVelRelative;
    public Vector3 COMRotVelRelative;

    private void Start()
    {
        parts = GetComponent<BodyParts>();
        physics = PhysicsUtils.get();
        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphereV = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Destroy(sphere.GetComponent<SphereCollider>());
        Destroy(sphereV.GetComponent<SphereCollider>());
        sphere.transform.localScale = new Vector3(.1f, .1f, .1f);
        sphereV.transform.localScale = new Vector3(.05f, .05f, .05f);
        sphere.GetComponent<MeshRenderer>().material.color = Color.red;
        sphereV.GetComponent<MeshRenderer>().material.color = Color.green;
        rigids = parts.getRigids();
    }

    private void FixedUpdate()
    {
        COM = physics.getCenterOfMass(rigids);
        COMVel = physics.getCenterOfMassVel(rigids);
        COMRotVel = physics.getCenterOfMassRotVel(rigids);
        COMVellMag = COMVel.sqrMagnitude;

        COMReal = COM;
        COMVelReal = COMVel;
        COMRotVelReal = COMRotVel;

        COM.z = -3;
        COMVel += COM;
        COMVel.z = -3;

        COMRotVel.x = COMRotVel.z + COM.x;
        COMRotVel.y = COM.y;
        COMRotVel.z = -3;

        sphere.transform.position = COM;
        sphereV.transform.position = COMRotVel;
        Debug.DrawLine(COM, COMVel, Color.blue);
        Debug.DrawLine(COM, COMRotVel, Color.green);
        Vector3 COMLine1 = COM;
        COMLine1.x -= .1f;
        COMLine1.y -= .1f;
        Vector3 COMLine2 = COM;
        COMLine2.x += .1f;
        COMLine2.y += .1f;
        Debug.DrawLine(COMLine1, COMLine2, Color.red);
        COMLine1.y += .2f;
        COMLine2.y -= .2f;
        Debug.DrawLine(COMLine1, COMLine2, Color.red);

        if (root != null)
        {
            COMRelative = physics.getCenterOfMass(rigids) - root.transform.position;
            COMVelRelative = root.transform.InverseTransformPoint(COMVelReal);
            COMRotVelRelative = Quaternion.Inverse(root.rotation) * COMRotVelReal;
        }
    }
}