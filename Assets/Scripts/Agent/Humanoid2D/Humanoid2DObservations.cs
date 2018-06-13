using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Humanoid2DObservations : MonoBehaviour
{
    public int observationsSpace;
    

    protected BodyParts bodyParts;
    protected List<float> observations = new List<float>();

    private List<GameObject> parts = new List<GameObject>();
    private PhysicsUtils physics;
    private readonly List<Vector3> positions = new List<Vector3>();
    public List<Rigidbody> observableRigids;
    private readonly List<Quaternion> rotations = new List<Quaternion>();
    private readonly List<Vector3> velocity = new List<Vector3>();
    private readonly List<Vector3> angVel = new List<Vector3>();
    public int decisionFrequency = 0;

    private readonly float maxPos = 100;
    private readonly float maxVel = 100;
    private readonly float minPos = -100;
    private readonly float minVel = -100;

    private void OnEnable()
    {
        bodyParts = GetComponent<BodyParts>();
        physics = PhysicsUtils.get();
        parts = bodyParts.getParts();
        observableRigids = bodyParts.ObservableRigids;


        //InvokeRepeating("Log", 0.0f, 1.0f);
    }

    protected virtual void Start()
    {
        observationsSpace = getObservationsSpace();
        
    }

    private void Update()
    {
    }

    private Vector3 getPosition(GameObject gameObject)
    {
        return gameObject.transform.localPosition;
    }

    private Quaternion getRotation(GameObject gameObject)
    {
        return gameObject.transform.localRotation;
    }

    private Vector3 getVelocity(GameObject gameObject)
    {
        return gameObject.GetComponent<Rigidbody>().velocity;
    }

    public List<Vector3> getObjectsPositions()
    {
        positions.Clear();
        foreach (var gameObject in observableRigids)
        {
            if (!gameObject.Equals(bodyParts.root))
                positions.Add(gameObject.transform.localPosition);
        }
        return positions;
    }

    public List<Quaternion> getObjectsRotations()
    {
        rotations.Clear();
        foreach (var gameObject in observableRigids)
        {
            if (!gameObject.name.Contains("_end"))
                rotations.Add(gameObject.transform.localRotation);
        }
        return rotations;
    }

    public List<Vector3> getObjectsVels()
    {
        velocity.Clear();
        foreach (var rigidbody in observableRigids) velocity.Add(rigidbody.velocity);
        return velocity;
    }

    public List<Vector3> getObjectsAngVels()
    {
        angVel.Clear();
        foreach (var rigidbody in observableRigids)
        {
            if (!rigidbody.name.Contains("_end"))
                angVel.Add(rigidbody.angularVelocity);
        }
        return angVel;
    }
    
    public int getObservationsSpace()
    {
        return getObservations().Count;
    }

    public virtual List<float> getObservations()
    {
        observations.Clear();

        var root = bodyParts.root;
        var rootPos = root.transform.position;
        addRootPos(rootPos);

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
            addRootPos(angVel);
        }
        

        return observations;
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
        //observations.Add(normVel(vel.y));
        observations.Add(normVel(vel.z));
    }

    public void addPos(Vector3 pos)
    {
        observations.Add(pos.x);
        //observations.Add(pos.y);
        observations.Add(pos.z);
    }

    public void addRootPos(Vector3 pos)
    {
        observations.Add(normPos(pos.x));
        //observations.Add(normPos(pos.y));
        //observations.Add(normPos(pos.z));
    }

    public void addEuler(Quaternion quaternion)
    {
        Vector3 rotEul = quaternion.eulerAngles;
        observations.Add(normEuler(rotEul.x));
        //observations.Add(normEuler(rotEul.y));
        //observations.Add(normEuler(rotEul.z));
    }

    public float normPos(float pos)
    {
        return (Mathf.Clamp(pos, minPos, maxPos) - minPos) / (maxPos - minPos);
    }

    public float normVel(float vel)
    {
        return (Mathf.Clamp(vel, minVel, maxVel) - minVel) / (maxVel - minVel);
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

    public void Log()
    {
        getObjectsPositions();
        getObjectsRotations();
        getObjectsVels();
        getObservationsSpace();
    }

}

