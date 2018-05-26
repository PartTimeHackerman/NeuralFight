using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class LockXY : MonoBehaviour
{
    private List<Rigidbody> rigids;
    private List<ConfigurableJoint> joints;

    void Start()
    {
        rigids = GetComponent<BodyParts>().getRigids();
        joints = GetComponent<BodyParts>().getJoints();

        foreach (Rigidbody rigid in rigids)
        {
            if (!joints.Contains(rigid.GetComponent<ConfigurableJoint>()) && !rigid.name.Contains("end") && !rigid.name.Contains("head"))
            {
                rigid.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ |
                                    RigidbodyConstraints.FreezePositionZ;

            }
        }
    }

    /*
    void LateUpdate()
    {
        foreach (Rigidbody rigid in rigids)
        {
            Vector3 angVel = rigid.angularVelocity;
            Quaternion angRot = rigid.rotation;
            angVel.z = 0;
            angVel.y = 0;
            angRot.z = 0;
            angRot.y = 0;
            rigid.angularVelocity = angVel;
            rigid.rotation = angRot;
        }
    }
    */
}
