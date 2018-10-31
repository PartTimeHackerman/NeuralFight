using System.Collections.Generic;
using UnityEngine;

public class TwoHandTakerRanged : MonoBehaviour
{
    public WeaponTwoHanded WeaponTwoHanded;
    private BodyParts BodyParts;
    private Rigidbody rHandRb;
    private Rigidbody lHandRb;

    private int frame = 0;
    private bool done = false;

    protected virtual void Start()
    {
        BodyParts = GetComponent<BodyParts>();

        foreach (KeyValuePair<string, Rigidbody> keyValuePair in BodyParts.getNamedRigids())
        {
            if (keyValuePair.Key.Contains("rhand")) rHandRb = keyValuePair.Value;
            if (keyValuePair.Key.Contains("lhand")) lHandRb = keyValuePair.Value;
        }

        foreach (JointInfo jointInfo in BodyParts.jointsInfos)
        {
            JointDrive jointSlerpDrive = jointInfo.joint.slerpDrive;
            jointSlerpDrive.positionSpring = 0f;
            jointSlerpDrive.positionDamper = 0f;
            jointInfo.joint.slerpDrive = jointSlerpDrive;
        }

        Vector3 lHandPosWep = lHandRb.transform.position;
        lHandPosWep.z = 0f;
        WeaponTwoHanded.lHandPlaceRb.position = lHandPosWep;
        Vector3 rot = lHandRb.rotation.eulerAngles;
        rot.z -= 90f;
        WeaponTwoHanded.lHandPlaceRb.rotation = Quaternion.Euler(rot);

        Vector3 rHandPosWep = WeaponTwoHanded.rHandPlaceRb.position;
        rHandPosWep.z = 0f;
        rHandRb.position = rHandPosWep;
    }

    private void FixedUpdate()
    {
        if (!done)
        {
            if (frame > 2)
            {
                addConfigurableJoint(rHandRb, WeaponTwoHanded.rHandPlaceRb);
                addConfigurableJoint(lHandRb, WeaponTwoHanded.lHandPlaceRb);
                WeaponTwoHanded.lHandPlaceRb.isKinematic = false;

                foreach (JointInfo jointInfo in BodyParts.jointsInfos)
                {
                    jointInfo.SetConfigurableJointSettings();
                }

                done = true;
            }

            frame++;
        }
        else
        {
            enabled = false;
        }
    }

    private void addConfigurableJoint(Rigidbody from, Rigidbody to)
    {
        ConfigurableJoint joint = from.gameObject.AddComponent<ConfigurableJoint>();
        joint.connectedBody = to;
        joint.xMotion = ConfigurableJointMotion.Locked;
        joint.yMotion = ConfigurableJointMotion.Locked;
        joint.zMotion = ConfigurableJointMotion.Locked;
        joint.angularXMotion = ConfigurableJointMotion.Locked;
        joint.angularYMotion = ConfigurableJointMotion.Locked;
        joint.angularZMotion = ConfigurableJointMotion.Locked;
        joint.rotationDriveMode = RotationDriveMode.Slerp;
        joint.projectionMode = JointProjectionMode.PositionAndRotation;
        joint.projectionDistance = 0f;
        joint.massScale = .1f;
        joint.connectedMassScale = 10f;
        joint.enablePreprocessing = false;
    }
}