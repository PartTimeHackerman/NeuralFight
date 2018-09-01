using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Observations : MonoBehaviour, IObservations
{
    public int observationsSpace;
    
    protected BodyParts bodyParts;
    protected List<string> obsToRemove = new List<string>();
    protected Dictionary<string, float> observationsNamed = new Dictionary<string, float>();
    private PhysicsUtils physics;
    public List<Rigidbody> observableRigids;
    public List<Rigidbody> rigids;

    private readonly float maxPos = 10;
    private readonly float maxVel = 100;
    private readonly float minPos = -10;
    private readonly float minVel = -100;
    private Rigidbody root;
/*

    public Observations(BodyParts bodyParts)
    {
        observationsNamed = new Dictionary<string, float>();
        this.bodyParts = bodyParts;
        physics = PhysicsUtils.get();
        observableRigids = bodyParts.ObservableRigids;
        rigids = bodyParts.getRigids();
        root = bodyParts.root;
        observationsSpace = getObservationsSpace();
    }
*/

    private void OnEnable()
    {
        bodyParts = GetComponent<BodyParts>();
        physics = PhysicsUtils.get();
        observableRigids = bodyParts.ObservableRigids;
        rigids = bodyParts.getRigids();
        root = bodyParts.root;
    }

    protected virtual void Start()
    {
        observationsSpace = getObservationsSpace();

    }

    public void getObjectsPositions()
    {
        foreach (var gameObject in observableRigids)
        {
            if (!gameObject.Equals(bodyParts.root))
            {
                Vector3 pos = root.transform.InverseTransformPoint(gameObject.transform.position);
                observationsNamed[gameObject.name + "_pos_x"] = pos.x;
                observationsNamed[gameObject.name + "_pos_y"] = pos.y;
            }
        }
    }

    public void getObjectsRotations()
    {
        foreach (var gameObject in observableRigids)
        {
            if (!gameObject.name.Contains("_end") && !gameObject.Equals(bodyParts.root))
            {
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
    }

    public void getObjectsVels()
    {
        foreach (var rigidbody in observableRigids)
        {
            Vector3 vel = normVecVel(rigidbody.velocity);
            observationsNamed[rigidbody.name + "_vel_x"] = vel.x;
            observationsNamed[rigidbody.name + "_vel_y"] = vel.y;

        }
    }

    public void getObjectsAngVels()
    {
        foreach (var rigidbody in observableRigids)
        {
            if (!rigidbody.name.Contains("_end"))
            {
                float rbAngVel = rigidbody.angularVelocity.z;
                rbAngVel = (Mathf.Clamp(rbAngVel, minVel, maxVel) - minVel) / (maxVel - minVel);
                observationsNamed[rigidbody.name + "_ang_vel"] = rbAngVel;

            }
        }
    }

    public int getObservationsSpace()
    {
        return getObservations().Count;
    }

    public int getObsSize()
    {
        return observationsSpace;
    }

    public virtual Dictionary<string, float> getObservationsNamed()
    {
        var rootPos = root.transform.position;
        observationsNamed["root_pos_x"] = normPos(rootPos.x);
        observationsNamed["root_pos_y"] = distToFloor();
        Quaternion quaternion = root.transform.rotation;
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
        
        removeParts();
        return observationsNamed;
    }

    public virtual List<float> getObservations()
    {
        getObservationsNamed();
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

    public float distToFloor()
    {
        int layerMask = 1 << root.gameObject.layer;
        layerMask = ~layerMask;
        RaycastHit hit;
        if (Physics.Raycast(root.transform.position, Vector3.down, out hit, 1000, layerMask))
            return hit.distance;
        else
            return 0;
    }

    public void addCOM()
    {
        Vector3 COM = physics.getCenterOfMass(rigids) - bodyParts.root.transform.position;
        Vector3 COMVel = physics.getCenterOfMassVel(rigids);
        Vector3 COMRotVel = Quaternion.Inverse(root.rotation) * physics.getCenterOfMassRotVel(rigids);
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

    public Vector3 normVecVel(Vector3 vel)
    {
        Vector3 nVel = new Vector3();
        nVel.x = (Mathf.Clamp(vel.x, minVel, maxVel) - minVel) / (maxVel - minVel);
        nVel.y = (Mathf.Clamp(vel.y, minVel, maxVel) - minVel) / (maxVel - minVel);
        nVel.z = (Mathf.Clamp(vel.z, minVel, maxVel) - minVel) / (maxVel - minVel);
        return nVel;
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

