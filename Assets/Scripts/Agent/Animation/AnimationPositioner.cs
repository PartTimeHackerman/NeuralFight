using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class AnimationPositioner : MonoBehaviour
{
    public BodyParts refBodyParts;
    public ReferenceObservations referenceObservations;
    public bool staticAnimation = false;
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

    private void FixedUpdate()
    {
        if (staticAnimation)
        {
            float refButtRotZ = refBodyParts.root.rotation.eulerAngles.x;
            Vector3 buttRot = bodyParts.root.rotation.eulerAngles;
            buttRot.z = -refButtRotZ;
            bodyParts.root.rotation = Quaternion.Euler(buttRot);
            bodyParts.root.position = refBodyParts.root.position;
        }

        if (debug)
        {
            
            setRotations();
            debug = false;
        }
    }

    void LateFixedUpdate()
    {
    }

    public void setRotations()
    {
        bodyParts.root.isKinematic = true;
        float refButtRotZ = refBodyParts.root.rotation.eulerAngles.x;
        Vector3 buttRot = bodyParts.root.rotation.eulerAngles;
        buttRot.z = -refButtRotZ;
        bodyParts.root.rotation = Quaternion.Euler(buttRot);
        Vector3 refRootPos = refBodyParts.root.position;
        refRootPos.x = 0;
        refRootPos.z = 0;
        bodyParts.root.position = refRootPos;
        foreach (JointInfo jointInfo in refBodyParts.jointsInfos)
        {
            float rot = jointInfo.transform.localRotation.eulerAngles.x;
            rot = rot < 180 ? -rot : (360 - rot);
            JointInfo modelJointInfo = namedJointInfos[jointInfo.name];
            Vector3 modelPartRot = modelJointInfo.transform.localRotation.eulerAngles;
            modelPartRot.z = rot;
            modelJointInfo.transform.localRotation = Quaternion.Euler(modelPartRot);

            //set target rot
            float tarRot = jointInfo.isBackwards ? rot: -rot;
            modelJointInfo.setConfigurableRot(new Vector3(tarRot, 0f, 0f));

            //namedJointInfos[jointInfo.name].GetComponent<Rigidbody>().isKinematic = true;
        }
        bodyParts.root.isKinematic = false;
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