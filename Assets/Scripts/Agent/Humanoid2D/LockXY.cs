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
    private BodyParts bodyParts;
    private Rigidbody root;
    private Quaternion qMult = new  Quaternion(1f,1f,1f,1f);

    void Start()
    {
        bodyParts = GetComponent<BodyParts>();
        rigids = GetComponent<BodyParts>().getRigids();
        joints = GetComponent<BodyParts>().getJoints();
        root = bodyParts.root;
        /*
        foreach (Rigidbody rigid in rigids)
        {
            if (!joints.Contains(rigid.GetComponent<ConfigurableJoint>()) && !rigid.name.Contains("end") && !rigid.name.Contains("head"))
            {
                rigid.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ |
                                    RigidbodyConstraints.FreezePositionZ;

            }
        }*/
    }

    void LateUpdate()
    {
        for (int i = rigids.Count - 1; i >= 0; i--)
        {
            Rigidbody rigid = rigids[i];
            //Vector3 angVel = rigid.angularVelocity;
            Quaternion angRot = rigid.rotation;
            //angVel.z = 0;
            //angVel.y = 0;
            angRot.x = 0f;
            angRot.y = 0f;
            //rigid.angularVelocity = angVel;
            rigid.transform.rotation = angRot;

            //float z = rigid.rotation.eulerAngles.z;
            //rigid.transform.Rotate(0, 0, -z);

        }

        /*
         Vector3 rootRot = bodyParts.root.rotation.eulerAngles;
        rootRot.y = 90f;
        bodyParts.root.transform.rotation = Quaternion.Euler(rootRot);
        */

    }
}
