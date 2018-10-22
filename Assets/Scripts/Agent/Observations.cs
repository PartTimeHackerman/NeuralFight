using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Observations : MonoBehaviour, IObservations
{
    public int observationsSpace;

    protected BodyParts bodyParts;
    protected List<string> obsToRemove = new List<string>();
    public Dictionary<string, float> observationsNamed = new Dictionary<string, float>();
    private PhysicsUtils physics;
    public List<Rigidbody> observableRigids;
    public List<Rigidbody> rigids;

    private readonly float maxPos = 100;
    private readonly float maxVel = 100;
    private readonly float minPos = -100;
    private readonly float minVel = -100;

    protected Rigidbody root;
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

    public void getObjectPosition(Transform transform)
    {
        Vector3 pos = root.transform.InverseTransformPoint(transform.position);
        observationsNamed[transform.name + "_pos_x"] = pos.x;
        observationsNamed[transform.name + "_pos_y"] = pos.y;
    }

    public void getObjectRotation(Rigidbody rigidbody)
    {
        Quaternion quaternion = Quaternion.Inverse(root.rotation) * rigidbody.transform.rotation;
        Vector3 rotEul = quaternion.eulerAngles;
        float rotAng = rotEul.z;
        float rotClamped = 0f;
        if (rotAng <= 180f)
            rotClamped = rotAng / 180f;
        else
            rotClamped = ((rotAng - 180f) / 180f) - 1f;

        observationsNamed[rigidbody.name + "_rot"] = rotClamped;
    }

    public void getObjectVel(Rigidbody rigidbody)
    {
        Vector3 vel = normVecVel(rigidbody.velocity);
        observationsNamed[rigidbody.name + "_vel_x"] = vel.x;
        observationsNamed[rigidbody.name + "_vel_y"] = vel.y;
    }

    public void getObjectAngVel(Rigidbody rigidbody)
    {
        float rbAngVel = rigidbody.angularVelocity.z;
        rbAngVel = Mathf.Clamp(rbAngVel, minVel, maxVel);
        observationsNamed[rigidbody.name + "_ang_vel"] = rbAngVel;
    }

    public void getObjPosRotVelAngVel()
    {
        for (int i = observableRigids.Count - 1; i >= 0; i--)
        {
            Rigidbody rigidbody = observableRigids[i];
            getObjectPosition(rigidbody.transform);
            getObjectRotation(rigidbody);
            getObjectVel(rigidbody);
            getObjectAngVel(rigidbody);
        }

        foreach (Transform ending in bodyParts.endings)
        {
            getObjectPosition(ending);
        }

        getObjectVel(root);
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


        getObjPosRotVelAngVel();
        addCOM();
        removeParts();
        removeInfsAndNans(observationsNamed);
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
        if (Physics.Raycast(root.transform.position, Vector3.down, out hit, 1000, layerMask,
            QueryTriggerInteraction.Ignore))
            return hit.distance;
        else
            return 0;
    }

    public void addCOM()
    {
        Vector3 COM;
        Vector3 COMVel;
        Vector3 COMAngVel;
        physics.getCenterOfMassAll(rigids, out COM, out COMVel, out COMAngVel);
        COM = COM - bodyParts.root.transform.position;
        COMAngVel = Quaternion.Inverse(root.rotation) * COMAngVel;
        observationsNamed["com_pos_x"] = COM.x;
        observationsNamed["com_pos_y"] = COM.y;
        observationsNamed["com_vel_x"] = COMVel.x;
        observationsNamed["com_vel_y"] = COMVel.y;
        observationsNamed["com_ang_vel"] = COMAngVel.z;
    }

    public float normPos(float pos)
    {
        return (Mathf.Clamp(pos, minPos, maxPos) - minPos) / (maxPos - minPos);
    }

    public Vector3 normVecVel(Vector3 vel)
    {
        Vector3 nVel = new Vector3();
        //nVel.x = (Mathf.Clamp(vel.x, minVel, maxVel) - minVel) / (maxVel - minVel);
        //nVel.y = (Mathf.Clamp(vel.y, minVel, maxVel) - minVel) / (maxVel - minVel);
        //nVel.z = (Mathf.Clamp(vel.z, minVel, maxVel) - minVel) / (maxVel - minVel);
        nVel.x = Mathf.Clamp(vel.x, minVel, maxVel);
        nVel.y = Mathf.Clamp(vel.y, minVel, maxVel);
        nVel.z = Mathf.Clamp(vel.z, minVel, maxVel);
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

    private void removeInfsAndNans(Dictionary<string, float> obs)
    {
        foreach (KeyValuePair<string, float> keyValuePair in obs)
        {
            float v = keyValuePair.Value;
            if (float.IsNaN(v) || float.IsInfinity(v) || float.IsNegativeInfinity(v) || float.IsPositiveInfinity(v))
            {
                obs[keyValuePair.Key] = 0f;
            }
        }
    }

    public void Log()
    {
        getObservationsSpace();
    }
}