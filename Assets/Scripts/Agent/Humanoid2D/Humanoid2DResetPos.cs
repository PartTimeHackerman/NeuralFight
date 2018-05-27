using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Humanoid2DResetPos : MonoBehaviour
{
    private bool backupped = false;
    private BodyParts bodyParts;

    private readonly Dictionary<GameObject, Vector3> defaultPos = new Dictionary<GameObject, Vector3>();
    private readonly Dictionary<GameObject, Quaternion> defaultRot = new Dictionary<GameObject, Quaternion>();
    private readonly Dictionary<Rigidbody, Vector3> defaultVel = new Dictionary<Rigidbody, Vector3>();

    private List<JointInfo> jointInfos;

    public long resets = 0;
    public long resetsBoundary = 2000000;
    public float scale;

    public int minRot = -10;
    public int maxRot = 10;
    public int minVel = -5000;
    public int maxVel = 5000;
    public float randomJointsPosForce = 1000;

    private Vector3 defRotation;

    private List<GameObject> parts;
    public bool reset;
    private List<Rigidbody> rigids;
    public float minRotScaled;
    public float maxRotScaled;
    public float minVelScaled;
    public float maxVelScaled;

    public bool randomJointsPos = false;
    public float minRandomJointsPos = -1f;
    public float maxRandomJointsPos = 1f;


    private void Start()
    {
        bodyParts = GetComponent<BodyParts>();
        parts = bodyParts.getParts();
        rigids = bodyParts.getRigids();
        defRotation = transform.rotation.eulerAngles;
        jointInfos = bodyParts.jointsInfos;
        backUpTransform();
    }


    private void Update()
    {
        if (reset)
        {
            ResetPosition();
            reset = false;
        }
    }

    private void backUpTransform()
    {
        foreach (var gameObject in parts)
        {
            var gameObjectTransform = gameObject.transform;
            defaultPos[gameObject] = gameObjectTransform.position;
            defaultRot[gameObject] = gameObjectTransform.rotation;
        }

        foreach (var rigidBody in rigids) defaultVel[rigidBody] = rigidBody.velocity;
    }

    public void ResetPosition()
    {
        resetTransform();
        resetTransform();
        resetTransform();
        

        scale = resets < resetsBoundary ? resets / (float) resetsBoundary + .3f : 1f;

        minRotScaled = minRot * scale;
        maxRotScaled = maxRot * scale;
        minVelScaled = minVel * scale;
        maxVelScaled = maxVel * scale;

        resetRandomXRot(minRotScaled, maxRotScaled);

        addRandomVel(minVelScaled, maxVelScaled);

        if (randomJointsPos)
            setRandomJointsPos();

        resets++;
        
    }

    private void resetTransform()
    {
        foreach (KeyValuePair<GameObject, Vector3> objPos in defaultPos) objPos.Key.transform.position = objPos.Value;
        foreach (KeyValuePair<GameObject, Quaternion> objRot in defaultRot)
            objRot.Key.transform.rotation = objRot.Value;
        resetVel();
    }

    public void resetVel()
    {
        foreach (KeyValuePair<Rigidbody, Vector3> objVel in defaultVel)
        {
            var kinematic = objVel.Key.isKinematic;
            objVel.Key.WakeUp();
            objVel.Key.isKinematic = true;
            objVel.Key.isKinematic = kinematic;
            objVel.Key.velocity = new Vector3(0, 0, 0);
        }
    }

    public void resetRandomXRot(float min, float max)
    {
        Quaternion quaternion = Quaternion.Euler(Random.Range(min, max), 0, 0);
        transform.rotation = quaternion;
    }

    public void addRandomVel(float min, float max)
    {
        var rigids = bodyParts.getRigids();
        //Rigidbody rigidbody = rigids[(int) Random.Range(0, rigids.Count)];
        int rn = 0;
        foreach (Rigidbody rigidbody in rigids)
        {
            float gotcha = Random.Range(0, 10);
            if ( gotcha > scale * 20)
                continue;
            rn++;
            //float yVel = Random.Range(min, max);
            //yVel = yVel < 0 ? yVel * .3f : yVel * 1.5f;
            Vector3 vel = new Vector3(0, Random.Range(min, max) * .6f, Random.Range(min, max));
            rigidbody.AddForce(vel * rigidbody.mass);
        }
    }

    public void setRandomJointsPos()
    {
        foreach (JointInfo jointInfo in jointInfos)
        {
            bool[] movableAxis = jointInfo.movableAxis;

            if (!movableAxis.Contains(true))
                continue;

            
            JointDrive jointSlerpDrive = jointInfo.joint.slerpDrive;
            jointSlerpDrive.positionSpring = randomJointsPosForce * jointInfo.maxPosSpring;
            jointSlerpDrive.positionDamper = 0;
            jointInfo.joint.slerpDrive = jointSlerpDrive;

            Vector3 angRot = new Vector3(0, 0, 0);
            if (movableAxis[0])
                angRot.x = getEuqlides(Random.Range(minRandomJointsPos, maxRandomJointsPos), jointInfo.angularLimits[0]);
            if (movableAxis[1])
                angRot.y = getEuqlides(Random.Range(minRandomJointsPos, maxRandomJointsPos), jointInfo.angularLimits[1]);
            if (movableAxis[2])
                angRot.z = getEuqlides(Random.Range(minRandomJointsPos, maxRandomJointsPos), jointInfo.angularLimits[2]);


            angRot.x = Mathf.Clamp(angRot.x, jointInfo.angularLimits[0][0], jointInfo.angularLimits[0][1]);
            angRot.y = Mathf.Clamp(angRot.y, jointInfo.angularLimits[1][0], jointInfo.angularLimits[1][1]);
            angRot.z = Mathf.Clamp(angRot.z, jointInfo.angularLimits[2][0], jointInfo.angularLimits[2][1]);
            //jointInfo.joint.transform.rotation = Quaternion.Euler(angRot);
            jointInfo.joint.targetRotation = Quaternion.Euler(angRot);

            /*
            jointSlerpDrive = jointInfo.joint.slerpDrive;
            jointSlerpDrive.positionSpring = jointInfo.maxPosSpring;
            jointSlerpDrive.positionDamper = jointInfo.maxPosDamper;
            jointInfo.joint.slerpDrive = jointSlerpDrive;
            */
            //if(jointInfo.joint.name == "rthigh") Debug.Log(angRot);
        }
    }

    public void resetJointForces()
    {
        foreach (JointInfo jointInfo in jointInfos)
        {
            bool[] movableAxis = jointInfo.movableAxis;

            if (!movableAxis.Contains(true))
                continue;

            JointDrive jointSlerpDrive = jointInfo.joint.slerpDrive;
            jointSlerpDrive.positionSpring = jointInfo.maxPosSpring;
            jointSlerpDrive.positionDamper = jointInfo.maxPosDamper;
            jointInfo.joint.slerpDrive = jointSlerpDrive;
        }
    }


    public void resetJointsPos()
    {
        foreach (JointInfo jointInfo in jointInfos)
        {
            bool[] movableAxis = jointInfo.movableAxis;

            if (!movableAxis.Contains(true))
                continue;

            float force = 0f;
            JointDrive jointSlerpDrive = jointInfo.joint.slerpDrive;
            jointSlerpDrive.positionSpring = force * jointInfo.maxPosSpring;
            jointSlerpDrive.positionDamper = force * jointInfo.maxPosDamper;
            jointInfo.joint.slerpDrive = jointSlerpDrive;
            
        }
    }

    private float getEuqlides(float action, float[] limits)
    {
        return action < 0 ? -action * limits[0] : action * limits[1];
    }
}