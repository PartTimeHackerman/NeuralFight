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
    protected PhysicsUtils physics;
    public List<Rigidbody> observableRigids;
    public List<Rigidbody> rigids;

    private Dictionary<string, BodyPart> namedObservableBodyParts;
    private Dictionary<string, BaseObservations> baseObservations = new Dictionary<string, BaseObservations>();

    private Dictionary<GameObject, Vector3> detachedPos = new Dictionary<GameObject, Vector3>();
    private Dictionary<GameObject, float> detachedRot = new Dictionary<GameObject, float>();
    private Dictionary<GameObject, Vector3> detachedVel = new Dictionary<GameObject, Vector3>();
    private Dictionary<GameObject, float> detachedAngVel = new Dictionary<GameObject, float>();
    
    private Dictionary<GameObject, string[]> PosStr = new Dictionary<GameObject, string[]>();
    private Dictionary<GameObject, string> RotStr = new Dictionary<GameObject, string>();
    private Dictionary<GameObject, string[]> VelStr = new Dictionary<GameObject, string[]>();
    private Dictionary<GameObject, string> AngVelStr = new Dictionary<GameObject, string>();
    private Dictionary<GameObject, string> GDStr = new Dictionary<GameObject, string>();
    
    private readonly float maxPos = 100;
    private readonly float maxVel = 100;
    private readonly float minPos = -100;
    private readonly float minVel = -100;

    protected Rigidbody root;

#if (UNITY_EDITOR)
    public bool debug = false;
    public DictionaryStringFloat observationsDebug = new DictionaryStringFloat();
#endif

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
        namedObservableBodyParts = bodyParts.namedObservableBodyParts;
        rigids = bodyParts.getRigids();
        root = bodyParts.root;

        
    }

    protected virtual void Start()
    {
        VelStr[root.gameObject] = new[] {root.name + "_vel_x", root.name + "_vel_y"};
        foreach (Rigidbody rigid in observableRigids)
        {
            PosStr[rigid.gameObject] = new[] {rigid.name + "_pos_x", rigid.name + "_pos_y"};
            RotStr[rigid.gameObject] = rigid.name + "_rot";
            VelStr[rigid.gameObject] = new[] {rigid.name + "_vel_x", rigid.name + "_vel_y"};
            AngVelStr[rigid.gameObject] = rigid.name + "_ang_vel";
        }
        foreach (var ending in bodyParts.endings)
        {
            PosStr[ending.gameObject] = new[] {ending.name + "_pos_x", ending.name + "_pos_y"};
            GDStr[ending.gameObject] = ending.name + "_gd";
        }
        observationsSpace = getObservationsSpace();
#if (UNITY_EDITOR)
        if (debug)
            InvokeRepeating("GetObservations", 0f, .1f);
#endif
        
    }

    public static Vector3 getObjectPosition(Transform transform, Rigidbody root)
    {
        Vector3 pos = root.transform.InverseTransformPoint(transform.position);
        return pos;
    }
    
    public void setObjectPosition(Rigidbody rigidbody)
    {
        Transform transform = rigidbody.transform;
        setObjectPosition(transform);
    }
    
    public virtual void setObjectPosition(Transform transform)
    {
        /*Vector3 pos;
        if (transform.CompareTag("detached"))
        {
            if (detachedPos.ContainsKey(transform.gameObject)) pos = detachedPos[transform.gameObject];
            else
            {
                pos = getObjectPosition(transform, root);
                detachedPos[transform.gameObject] = pos;
            }
        }
        else
        {
            pos = getObjectPosition(transform, root);
        }*/
        
        //bool detached = detachedPos.ContainsKey(transform.gameObject);
        //pos = detached ? detachedPos[transform.gameObject] : getObjectPosition(transform, root);
        //if (transform.CompareTag("detached") && !detached) detachedPos[transform.gameObject] = pos;
        var pos = getObjectPosition(transform, root);
        string[] xy = PosStr[transform.gameObject];
        observationsNamed[xy[0]] = pos.x;
        observationsNamed[xy[1]] = pos.y;
    }

    public static float getObjectRotation(Rigidbody rigidbody, Rigidbody root)
    {
        Quaternion quaternion = Quaternion.Inverse(root.rotation) * rigidbody.transform.rotation;
        Vector3 rotEul = quaternion.eulerAngles;
        float rotAng = rotEul.z;
        float rotClamped = 0f;
        if (rotAng <= 180f)
            rotClamped = rotAng / 180f;
        else
            rotClamped = ((rotAng - 180f) / 180f) - 1f;

        return rotClamped;
    }
    
    public virtual void setObjectRotation(Rigidbody rigidbody)
    {
        //bool detached = detachedRot.ContainsKey(rigidbody.gameObject);
        //float rot =  detached? detachedRot[rigidbody.gameObject] : getObjectRotation(rigidbody, root);
        //if (rigidbody.CompareTag("detached") && !detached ) detachedRot[rigidbody.gameObject] = rot;
        var rot = getObjectRotation(rigidbody, root);
        observationsNamed[RotStr[rigidbody.gameObject]] = rot;
    }

    public static Vector3 getObjectVel(Rigidbody rigidbody)
    {
        return rigidbody.velocity;
    }
    
    public virtual void setObjectVel(Rigidbody rigidbody)
    {
        //bool detached = detachedVel.ContainsKey(rigidbody.gameObject);
        //Vector3 vel = detached ? detachedVel[rigidbody.gameObject] : getObjectVel(rigidbody);
        //if (rigidbody.CompareTag("detached") && !detached) detachedVel[rigidbody.gameObject] = Vector3.zero;
        var vel = getObjectVel(rigidbody);
        string[] xy = VelStr[rigidbody.gameObject];
        observationsNamed[xy[0]] = vel.x;
        observationsNamed[xy[1]] = vel.y;
    }

    public static float getObjectAngVel(Rigidbody rigidbody)
    {
        float rbAngVel = rigidbody.angularVelocity.z;
        return rbAngVel;
    }
    
    public virtual void setObjectAngVel(Rigidbody rigidbody)
    {
        //bool detached = detachedAngVel.ContainsKey(rigidbody.gameObject);
        //float rbAngVel  = detached ? detachedAngVel[rigidbody.gameObject] : getObjectAngVel(rigidbody);
        //if (rigidbody.CompareTag("detached") && !detached ) detachedAngVel[rigidbody.gameObject] = 0f;
        float rbAngVel = getObjectAngVel(rigidbody);
        observationsNamed[AngVelStr[rigidbody.gameObject]] = rbAngVel;
    }

    public virtual void GetEndingsGroundDist()
    {
        foreach (var ending in bodyParts.endings)
        {
            observationsNamed[GDStr[ending.gameObject]] = distToFloor(ending);
        }
    }

    public void GetObjPosRotVelAngVel()
    {
        for (int i = observableRigids.Count - 1; i >= 0; i--)
        {
            Rigidbody rigidbody = observableRigids[i];
            setObjectPosition(rigidbody);
            setObjectRotation(rigidbody);
            setObjectVel(rigidbody);
            setObjectAngVel(rigidbody);
        }

        foreach (Transform ending in bodyParts.endings)
        {
            setObjectPosition(ending);
        }

        setObjectVel(root);
    }

    public int getObservationsSpace()
    {
        return GetObservations().Count;
    }

    public int getObsSize()
    {
        return observationsSpace;
    }

    public virtual Dictionary<string, float> getObservationsNamed()
    {
        var rootPos = root.transform.position;
        observationsNamed["root_pos_x"] = normPos(rootPos.x);
        observationsNamed["root_pos_y"] = distToFloor(root.transform);
        Quaternion quaternion = root.transform.rotation;
        Vector3 rotEul = quaternion.eulerAngles;
        float rotAng = rotEul.z;
        float rotClamped = 0f;
        if (rotAng <= 180f)
            rotClamped = rotAng / 180f;
        else
            rotClamped = ((rotAng - 180f) / 180f) - 1f;
        observationsNamed["root_rot"] = rotClamped;


        GetObjPosRotVelAngVel();
        GetCOM();
        GetEndingsGroundDist();
        removeParts();
        removeInfsAndNans(observationsNamed);

#if (UNITY_EDITOR)
        if (debug)
        {
            foreach (KeyValuePair<string, float> keyValuePair in observationsNamed)
            {
                observationsDebug[keyValuePair.Key] = keyValuePair.Value;
            }
        }
#endif
        return observationsNamed;
    }

    public virtual List<float> GetObservations()
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

    public static float distToFloor(Transform transform)
    {
        int layerMask = 1 << transform.gameObject.layer;
        layerMask = ~layerMask;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1000, layerMask, QueryTriggerInteraction.Ignore))
        {
            //Debug.DrawRay(transform.position, Vector3.down * hit.distance);
            return hit.distance;
        }
        else
        {
            return 0;
        }
    }

    public virtual void GetCOM()
    {
        Vector3 COM;
        Vector3 COMVel;
        Vector3 COMAngVel;
        physics.getCenterOfMassAll(rigids, out COM, out COMVel, out COMAngVel);
        COM = root.transform.InverseTransformPoint(COM);
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
    

    public class BaseObservations
    {
        public BodyPart BodyPart;
        public Rigidbody root;
        public Vector3 Pos;
        public float Rot;
        public Vector3 Vel;
        public float AngVel;

        public BaseObservations(BodyPart bodyPart, Rigidbody root)
        {
            BodyPart = bodyPart;
            this.root = root;

            Pos = getObjectPosition(bodyPart.transform, root);
            Rot = getObjectRotation(bodyPart.Rigidbody, root);
            Vel = getObjectVel(bodyPart.Rigidbody);
            AngVel = getObjectAngVel(bodyPart.Rigidbody);
        }

    }
}