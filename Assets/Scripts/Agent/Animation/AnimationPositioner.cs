using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class AnimationPositioner : MonoBehaviour
{
    public BodyParts refBodyParts;
    public bool debug = false;
    public ReferenceObservations referenceObservations;
    private BodyParts bodyParts;
    private Dictionary<string, JointInfo> namedJointInfos = new Dictionary<string, JointInfo>();
    private Dictionary<string, Rigidbody> namedRigids;
    private List<Velocity> refVels;
    private Dictionary<string, Velocity> namedRefVels = new Dictionary<string, Velocity>();

    void Start()
    {
        bodyParts = GetComponent<BodyParts>();
        namedRigids = bodyParts.getNamedRigids();
        /*refVels = refBodyParts.GetComponentsInChildren<Velocity>().ToList();
        foreach (Velocity velocity in refVels)
        {
            namedRefVels[velocity.name] = velocity;
        }*/
        foreach (JointInfo jointInfo in bodyParts.jointsInfos)
        {
            namedJointInfos[jointInfo.name] = jointInfo;
        }
    }

    void LateFixedUpdate()
    {
        if (debug)
        {
            setRotations();
            setVelocities();
        }
    }

    public void setRotations()
    {

        foreach (JointInfo jointInfo in refBodyParts.jointsInfos)
        {

            float rot = jointInfo.transform.rotation.eulerAngles.x;
            rot = rot < 180 ? -rot : (360 - rot);
            Vector3 modelPartRot = namedJointInfos[jointInfo.name].transform.rotation.eulerAngles;
            modelPartRot.z = rot;
            namedJointInfos[jointInfo.name].transform.rotation = Quaternion.Euler(modelPartRot);
            //namedJointInfos[jointInfo.name].GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    public void setRotationsRigids()
    {
        foreach (KeyValuePair<string, Rigidbody> namedRigid in namedRigids)
        {
            namedRigid.Value.transform.rotation = referenceObservations.rbRotations[namedRigid.Key];
        }
    }

    public void setVelocities()
    {
        foreach (KeyValuePair<string, Rigidbody> namedRigid in namedRigids)
        {
            //namedRigid.Value.velocity = referenceObservations.velocities[namedRigid.Key];
            namedRigid.Value.angularVelocity = referenceObservations.angularVelocities[namedRigid.Key];
        }
    }
}
