using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Observations : MonoBehaviour, IObservations
{
    public int observationsSpace;


    protected BodyParts bodyParts;
    protected List<float> observations = new List<float>();
    protected List<string> obsToRemove = new List<string>();
    protected Dictionary<string, float> observationsNamed = new Dictionary<string, float>();
    private List<GameObject> parts = new List<GameObject>();
    private PhysicsUtils physics;
    private readonly List<Vector3> positions = new List<Vector3>();
    public List<Rigidbody> observableRigids;
    public List<Rigidbody> rigids;
    private readonly List<Quaternion> rotations = new List<Quaternion>();
    private readonly List<Vector3> velocity = new List<Vector3>();
    private readonly List<Vector3> angVel = new List<Vector3>();
    public int decisionFrequency = 0;

    private readonly float maxPos = 10;
    private readonly float maxVel = 100;
    private readonly float minPos = -10;
    private readonly float minVel = -100;
    private Rigidbody root;

    private void OnEnable()
    {
        bodyParts = GetComponent<BodyParts>();
        physics = PhysicsUtils.get();
        parts = bodyParts.getParts();
        observableRigids = bodyParts.ObservableRigids;
        rigids = bodyParts.getRigids();
        root = bodyParts.root;

        //InvokeRepeating("Log", 0.0f, 1.0f);
    }

    protected virtual void Start()
    {
        observationsSpace = getObservationsSpace();

    }

    public List<Vector3> getObjectsPositions()
    {
        positions.Clear();
        foreach (var gameObject in observableRigids)
        {
            if (!gameObject.Equals(bodyParts.root))
            {
                Vector3 pos = root.transform.InverseTransformPoint(gameObject.transform.position);
                positions.Add(root.transform.InverseTransformPoint(gameObject.transform.position));
                observationsNamed[gameObject.name + "_pos_x"] = pos.x;
                observationsNamed[gameObject.name + "_pos_y"] = pos.y;
            }
        }
        return positions;
    }

    public List<Quaternion> getObjectsRotations()
    {
        rotations.Clear();
        foreach (var gameObject in observableRigids)
        {
            if (!gameObject.name.Contains("_end") && !gameObject.Equals(bodyParts.root))
            {
                rotations.Add(Quaternion.Inverse(root.rotation) * gameObject.transform.rotation);
                Quaternion quaternion = Quaternion.Inverse(root.rotation) * gameObject.transform.rotation;
                Vector3 rotEul = quaternion.eulerAngles;
                float rotAng = rotEul.z;
                float rotClamped = 0f;
                if (rotAng <= 180f)
                    rotClamped = rotAng / 180f;
                else
                    rotClamped = ((rotAng - 180f) / 180f) - 1f;

                observationsNamed[gameObject.name + "_rot"] = rotClamped;
            }
        }
        return rotations;
    }

    public List<Vector3> getObjectsVels()
    {
        velocity.Clear();
        foreach (var rigidbody in observableRigids)
        {
            velocity.Add(rigidbody.velocity);
            Vector3 vel = normVecVel(rigidbody.velocity);
            observationsNamed[rigidbody.name + "_vel_x"] = vel.x;
            observationsNamed[rigidbody.name + "_vel_y"] = vel.y;

        }
        return velocity;
    }

    public List<Vector3> getObjectsAngVels()
    {
        angVel.Clear();
        foreach (var rigidbody in observableRigids)
        {
            if (!rigidbody.name.Contains("_end"))
            {
                angVel.Add(rigidbody.angularVelocity);
                float rbAngVel = rigidbody.angularVelocity.z;
                rbAngVel = (Mathf.Clamp(rbAngVel, minVel, maxVel) - minVel) / (maxVel - minVel);
                //observations.Add(rbAngVel);
                observationsNamed[rigidbody.name + "_ang_vel"] = rbAngVel;

            }
        }
        return angVel;
    }

    public int getObservationsSpace()
    {
        return getObservations().Count;
    }

    public int getObsSize()
    {
        return observationsSpace;
    }

    public virtual List<float> getObservations()
    {
        observations.Clear();


        var rootPos = root.transform.position;
        //addRootPos(rootPos);
        observationsNamed["root_pos_x"] = normPos(rootPos.x);
        observationsNamed["root_pos_y"] = normPos(rootPos.y);
        Quaternion quaternion = root.transform.rotation;
        //addEuler(quaternion);
        Vector3 rotEul = quaternion.eulerAngles;
        float rotAng = rotEul.z;
        float rotClamped = 0f;
        if (rotAng <= 180f)
            rotClamped = rotAng / 180f;
        else
            rotClamped = ((rotAng - 180f) / 180f) - 1f;
        observationsNamed["root_rot"] = rotClamped;


        getObjectsPositions();
        getObjectsRotations();
        getObjectsVels();
        getObjectsAngVels();
        addCOM();

        /*observations.Clear();
        foreach (var position in getObjectsPositions())
        {
            addPos(position);
        }

        foreach (var rotation in getObjectsRotations())
        {
            addEuler(rotation);
        }

        foreach (var velocity in getObjectsVels())
        {
            addVel(velocity);
        }

        foreach (var angVel in getObjectsAngVels())
        {
            observations.Add(normVel(angVel.z));
        }

        addCOM();*/
        removeParts();

        return observationsNamed.Select(kv => kv.Value).ToList();
    }

    public void addToRemove(string[] toRemove)
    {
        obsToRemove.AddRange(toRemove);
    }

    private void removeParts()
    {
        foreach (string obs in obsToRemove)
        {
            observationsNamed.Remove(obs);
        }
    }

    public void addQuaternion(Quaternion quaternion)
    {
        observations.Add(quaternion.x);
        observations.Add(quaternion.y);
        observations.Add(quaternion.z);
        observations.Add(quaternion.w);
    }

    public void addVel(Vector3 vel)
    {
        observations.Add(normVel(vel.x));
        observations.Add(normVel(vel.y));
        //observations.Add(normVel(vel.z));
    }

    public void addPos(Vector3 pos)
    {
        observations.Add(pos.x);
        observations.Add(pos.y);
        //observations.Add(pos.z);
    }

    public void addRootPos(Vector3 pos)
    {
        //observations.Add(normPos(pos.x));
        observations.Add(normPos(pos.y));
        //observations.Add(normPos(pos.z));
    }

    public void addEuler(Quaternion quaternion)
    {
        Vector3 rotEul = quaternion.eulerAngles;
        float rotAng = rotEul.z;
        float rotClamped = 0f;
        if (rotAng <= 180f)
            rotClamped = rotAng / 180f;
        else
            rotClamped = ((rotAng - 180f) / 180f) - 1f;
        observations.Add(rotClamped);
    }

    public void addCOM()
    {
        Vector3 COM = physics.getCenterOfMass(rigids) - bodyParts.root.transform.position;
        //Vector3 COMVel = root.transform.InverseTransformPoint(physics.getCenterOfMassVel(rigids));
        Vector3 COMVel = physics.getCenterOfMassVel(rigids);
        Vector3 COMRotVel = Quaternion.Inverse(root.rotation) * physics.getCenterOfMassRotVel(rigids);
        //observations.Add(COM.x);
        //observations.Add(COM.y);
        //observations.Add(COMVel.x);
        //observations.Add(COMVel.y);
        //observations.Add(COMRotVel.z);
        observationsNamed["com_pos_x"] = COM.x;
        observationsNamed["com_pos_y"] = COM.y;
        observationsNamed["com_vel_x"] = COMVel.x;
        observationsNamed["com_vel_y"] = COMVel.y;
        observationsNamed["com_ang_vel"] = COMRotVel.z;
    }

    public float normPos(float pos)
    {
        return (Mathf.Clamp(pos, minPos, maxPos) - minPos) / (maxPos - minPos);
    }

    public float normVel(float vel)
    {
        return vel;// (Mathf.Clamp(vel, minVel, maxVel) - minVel) / (maxVel - minVel);
    }

    public Vector3 normVecVel(Vector3 vel)
    {
        Vector3 nVel = new Vector3();
        nVel.x = (Mathf.Clamp(vel.x, minVel, maxVel) - minVel) / (maxVel - minVel);
        nVel.y = (Mathf.Clamp(vel.y, minVel, maxVel) - minVel) / (maxVel - minVel);
        nVel.z = (Mathf.Clamp(vel.z, minVel, maxVel) - minVel) / (maxVel - minVel);
        return nVel;
    }

    public float normEuler(float rot)
    {
        rot = rot % 360;
        return rot / 360f;
    }

    public Vector3 getCenterOfMass()
    {
        return physics.getCenterOfMass(observableRigids);
    }

    public void logNamedObs()
    {
        string str = "";
        foreach (KeyValuePair<string, float> keyValuePair in observationsNamed)
        {
            str += "[ " + keyValuePair.Key + ": " + keyValuePair.Value + " ] ";
        }
        Debug.Log(str);
    }

    public void Log()
    {
        getObjectsPositions();
        getObjectsRotations();
        getObjectsVels();
        getObservationsSpace();
    }

}

