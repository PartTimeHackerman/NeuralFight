using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class ReferenceObservations : MonoBehaviour, ILateFixedUpdate
{


    public Dictionary<string, Vector3> velocities = new Dictionary<string, Vector3>();
    public Dictionary<string, Vector3> angularVelocities = new Dictionary<string, Vector3>();
    public Dictionary<string, float> relativeRots = new Dictionary<string, float>();
    public Dictionary<string, Vector3> endPositions = new Dictionary<string, Vector3>();
    public Vector3 COM = new Vector3();
    public Dictionary<string, Quaternion> rbRotations = new Dictionary<string, Quaternion>();

    private Dictionary<string, Vector3> previousPositions = new Dictionary<string, Vector3>();
    private Dictionary<string, Quaternion> lastRotations = new Dictionary<string, Quaternion>();
    private BodyParts bodyParts;
    private Dictionary<string, Rigidbody> namedRigids;
    private Rigidbody root;
    private PhysicsUtils physics;

    void Start()
    {
        bodyParts = GetComponent<BodyParts>();
        namedRigids = bodyParts.getNamedRigids();
        root = bodyParts.root;
        physics = PhysicsUtils.get();
    }

    void FixedUpdate()
    {
        foreach (KeyValuePair<string, Rigidbody> namedRigid in namedRigids)
        {
            previousPositions[namedRigid.Key] = namedRigid.Value.transform.position;
            lastRotations[namedRigid.Key] = namedRigid.Value.transform.rotation;
        }

    }

    public void LateFixedUpdate()
    {
        foreach (KeyValuePair<string, Rigidbody> namedRigid in namedRigids)
        {
            Vector3 vel = (namedRigid.Value.transform.position - previousPositions[namedRigid.Key]) / Time.fixedDeltaTime;
            vel.z = 0f;
            velocities[namedRigid.Key] = vel;

            getAngVel(namedRigid.Key);
            relativeRots[namedRigid.Key] = getRelativeRot(namedRigid.Value, root, true);

            if (namedRigid.Key.Contains("_end"))
                endPositions[namedRigid.Key] = root.transform.InverseTransformPoint(namedRigid.Value.transform.position);

            float rot = namedRigid.Value.transform.rotation.eulerAngles.x;
            rot = rot < 180 ? -rot : (360 - rot);
            Vector3 rbRot = Vector3.zero;
            rbRot.z = rot;
            rbRotations[namedRigid.Key] = Quaternion.Euler(rbRot);


            COM = physics.getCenterOfMass(bodyParts.getRigids()) - bodyParts.root.transform.position;
        }

    }

    public void getAngVel(string rbName)
    {
        Vector3 angVel;
        Quaternion deltaRotation = transform.rotation * Quaternion.Inverse(lastRotations[rbName]);
        float angle = 0.0f;
        Vector3 axis = Vector3.zero;
        deltaRotation.ToAngleAxis(out angle, out axis);
        angle *= Mathf.Deg2Rad;
        if (angle > 1 || angle < -1)
        {
            Vector3 eulerRotation = new Vector3(
                Mathf.DeltaAngle(0, Mathf.Round(deltaRotation.eulerAngles.x)),
                Mathf.DeltaAngle(0, Mathf.Round(deltaRotation.eulerAngles.y)),
                Mathf.DeltaAngle(0, Mathf.Round(deltaRotation.eulerAngles.z)));
            angVel = eulerRotation / Time.fixedDeltaTime * Mathf.Deg2Rad;
        }
        else
            angVel = axis * angle / Time.fixedDeltaTime;
        angVel.x = 0;
        angVel.y = 0;
        angularVelocities[rbName] = angVel;

    }

    private float getRelativeRot(Rigidbody rigidbody, Rigidbody root, bool isRef)
    {
        float rotAng;
        float rotClamped = 0f;
        Vector3 rot = (Quaternion.Inverse(root.rotation) * rigidbody.transform.rotation).eulerAngles;

        if (isRef)
            rotAng = rot.x;
        else
            rotAng = rot.z;

        if (rotAng <= 180f)
            rotClamped = rotAng / 180f;
        else
            rotClamped = ((rotAng - 180f) / 180f) - 1f;


        return isRef ? -rotClamped : rotClamped;
    }
}
