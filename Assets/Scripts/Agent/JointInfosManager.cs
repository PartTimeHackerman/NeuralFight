using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class JointInfosManager
{
    private BodyParts bodyParts;
    private List<JointInfo> jointInfos;

    public JointInfosManager(BodyParts bodyParts)
    {
        this.bodyParts = bodyParts;
        jointInfos = bodyParts.jointsInfos;
    }

    public void disableJoints()
    {
        foreach (JointInfo jointInfo in jointInfos)
        {
            JointDrive jointSlerpDrive = jointInfo.joint.slerpDrive;
            jointSlerpDrive.positionSpring = 0f;
            jointSlerpDrive.positionDamper = 0f;
            jointInfo.joint.slerpDrive = jointSlerpDrive;
        }
    }

    public void enableJoints()
    {
        foreach (JointInfo jointInfo in jointInfos)
        {
            jointInfo.SetConfigurableJointSettings();
        }
    }

    public void setJointsJointsForces(float force)
    {
        foreach (JointInfo jointInfo in jointInfos)
        {
            JointDrive jointSlerpDrive = jointInfo.joint.slerpDrive;
            jointSlerpDrive.positionSpring = jointInfo.maxForce * jointInfo.totalMass;
            jointSlerpDrive.positionDamper = jointInfo.maxForce * jointInfo.totalMass * force;
            jointInfo.joint.slerpDrive = jointSlerpDrive;
        }
    }

}
