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
        foreach (KeyValuePair<string, Rigidbody> namedRigid in refBodyParts.getNamedRigids())
        {

            float rot = namedRigid.Value.transform.rotation.eulerAngles.x;
            rot = rot < 180 ? -rot : (360 - rot);
            Vector3 modelPartRot = bodyParts.getNamedRigids()[namedRigid.Key].transform.rotation.eulerAngles;
            modelPartRot.z = rot;
            bodyParts.getNamedRigids()[namedRigid.Key].transform.rotation = Quaternion.Euler(modelPartRot);
            //namedJointInfos[jointInfo.name].GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    public void setVelocities()
    {
        foreach (KeyValuePair<string, Velocity> namedRefVel in namedRefVels)
        {
            Vector3 refVel = namedRefVel.Value.velocity;
            Vector3 refAngVel = namedRefVel.Value.angularVelocity;
            refVel.z = 0;
            refAngVel.x = 0;
            refAngVel.y = 0;
            //namedRigids[namedRefVel.Key].velocity = refVel;
            namedRigids[namedRefVel.Key].angularVelocity = refAngVel;
        }
    }
}
