using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using UnityEngine;

public class BodyParts : MonoBehaviour
{
    private readonly List<ConfigurableJoint> joints = new List<ConfigurableJoint>();

    protected readonly Dictionary<ConfigurableJoint, bool[]> moveableJoints =
        new Dictionary<ConfigurableJoint, bool[]>();

    public List<JointInfo> jointsInfos = new List<JointInfo>();
    public Dictionary<string, JointInfo> namedJoints = new Dictionary<string, JointInfo>();
    protected Dictionary<string, GameObject> namedParts = new Dictionary<string, GameObject>();
    private readonly Dictionary<string, Rigidbody> namedRigids = new Dictionary<string, Rigidbody>();
    protected List<GameObject> parts = new List<GameObject>();
    private readonly List<Rigidbody> rigids = new List<Rigidbody>();
    private readonly List<Rigidbody> observableRigids = new List<Rigidbody>();
    public readonly List<Transform> endings = new List<Transform>();
    public List<GameObject> editableParts = new List<GameObject>();

    public List<Rigidbody> ObservableRigids => observableRigids;
    public Dictionary<string, BodyPart> namedObservableBodyParts = new Dictionary<string, BodyPart>();
    public Dictionary<string, BodyPart> AllBodyParts = new Dictionary<string, BodyPart>();
    
    
    public Rigidbody root;
    public int jointsTotal;
    public int partsTotal;
    public int rigidsTotal;
    private Vector3 COM { get; set; }
    public float totalRigidsMass;
    public float height;

    protected void Awake()
    {
        ConfigurableJoint[] jointsArr = GetComponentsInChildren<ConfigurableJoint>();
        jointsInfos.AddRange(GetComponentsInChildren<JointInfo>());
        jointsInfos = jointsInfos.OrderBy(joint => joint.joint.name).ToList();

        foreach (var joint in jointsArr) joints.Add(joint);
        foreach (JointInfo jointInfo in jointsInfos) namedJoints[jointInfo.name] = jointInfo;
        foreach (JointInfo jointInfo in jointsInfos)
        {
            if (!jointInfo.name.Contains("lwaist"))
            {
                editableParts.Add(jointInfo.gameObject);
            }
        }

        foreach (var child in GetComponentsInChildren<Transform>())
        {
            if (child.GetComponent<MeshRenderer>() != null || child.name.Contains("mesh"))
            {
                continue;
            }
            namedParts.Add(child.gameObject.name, child.gameObject);
            parts.Add(child.gameObject);
            if (child.name.Contains("_end"))
                endings.Add(child);
        }

        Rigidbody[] rigidsArr = GetComponentsInChildren<Rigidbody>();
        foreach (var rigidBody in rigidsArr)
        {
            namedRigids.Add(rigidBody.name, rigidBody);
            rigids.Add(rigidBody);
        }

        parts.Remove(gameObject);
        namedParts.Remove(gameObject.name);

        Dictionary<ConfigurableJoint, bool[]> movableJoints = getMovableJoints();
        foreach (var joint in getJoints())
        {
            bool[] movableAxis = {false, false, false};
            if (joint.angularXMotion != ConfigurableJointMotion.Locked)
                movableAxis[0] = true;
            if (joint.angularYMotion != ConfigurableJointMotion.Locked)
                movableAxis[1] = true;
            if (joint.angularZMotion != ConfigurableJointMotion.Locked)
                movableAxis[2] = true;

            if (movableAxis.Contains(true))
                movableJoints[joint] = movableAxis;

        }

        totalRigidsMass = rigids.Sum(r => r.mass);

        foreach (Rigidbody rigid in rigids)
        {
            
            if (rigid.GetComponent<JointInfo>() != null && rigid.GetComponent<JointInfo>().useNeural)
                observableRigids.Add(rigid);
        }
        //observableRigids.Add(root);

        List<BodyPart> bodyParts = GetComponentsInChildren<BodyPart>().ToList();
        foreach (BodyPart bodyPart in bodyParts)
        {
            AllBodyParts[bodyPart.name] = bodyPart;
            if (bodyPart.GetComponent<JointInfo>() != null && bodyPart.GetComponent<JointInfo>().useNeural)
                namedObservableBodyParts[bodyPart.name] = bodyPart;
        }

        height = getHeight();
        
        setKinematic(true);
    }


    private void Start()
    {
        partsTotal = parts.Count;
        rigidsTotal = rigids.Count;
        jointsTotal = joints.Count;
    }

    public void setKinematic(bool kinematic)
    {
        foreach (Rigidbody rigid in rigids)
        {
            rigid.isKinematic = kinematic;
        }
    }
    
    public void SetEnableJoints(bool enable)
    {
        List<BodyPart> parts = namedObservableBodyParts.Values.ToList();
        foreach (BodyPart bodyPart in parts)
        {
            if (bodyPart.partEnabled)
            {
                if (bodyPart.JointInfo.isEnabled && !enable) bodyPart.JointInfo.Disable();
                if (!bodyPart.JointInfo.isEnabled && enable) bodyPart.JointInfo.Enable();
            }
        }
    }

    public List<GameObject> getParts()
    {
        return parts;
    }

    public Dictionary<ConfigurableJoint, bool[]> getMovableJoints()
    {
        return moveableJoints;
    }

    public Dictionary<string, GameObject> getNamedParts()
    {
        return namedParts;
    }

    public Dictionary<string, Rigidbody> getNamedRigids()
    {
        return namedRigids;
    }

    public List<ConfigurableJoint> getJoints()
    {
        return joints;
    }

    public List<Rigidbody> getRigids()
    {
        return rigids;
    }

    private float getHeight()
    {
        float height = 0;
        foreach (Rigidbody rigid in rigids)
            height = rigid.transform.position.y > height ? rigid.transform.position.y : height;
        return height;
    }
}