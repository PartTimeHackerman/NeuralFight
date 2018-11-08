using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionsAngPos : MonoBehaviour, IActions
{
    public int actionsSpace;
    public bool VelPos = false;
    protected BodyParts bodyParts;
    public List<JointInfo> jointInfos = new List<JointInfo>();
    public List<JointInfo> allJointInfos;


    private void Start()
    {
        bodyParts = GetComponent<BodyParts>();
        allJointInfos = bodyParts.jointsInfos;
        foreach (JointInfo jointInfo in allJointInfos)
        {
            if (jointInfo.useNeural)
            {
                jointInfos.Add(jointInfo);
            }
        }
        actionsSpace = getActionsSpace();
    }


    public void applyActions(List<float> actions)
    {
        if (VelPos)
        {
            applyVel(actions);
        }
        else
            applyPos(actions);
    }

    protected virtual void applyPos(List<float> actions)
    {
        var actionIdx = 0;
        var size = actions.Count;

        if (size != actionsSpace)
        {
            Debug.Log("Wrong actions size: " + size + " != " + actionsSpace);
            return;
        }

        foreach (JointInfo jointInfo in jointInfos)
        {
            bool[] movableAxis = jointInfo.movableAxis;

            if (!movableAxis.Contains(true))
                continue;

            float force = 1; //(actions[actionIdx++] + 1) / 2;

            Vector3 angRot = new Vector3(0, 0, 0);
            if (movableAxis[0])
                angRot.x = getEuqlides(actions[actionIdx++], jointInfo.angularLimits[0]);
            if (movableAxis[1])
                angRot.y = getEuqlides(actions[actionIdx++], jointInfo.angularLimits[1]);
            if (movableAxis[2])
                angRot.z = getEuqlides(actions[actionIdx++], jointInfo.angularLimits[2]);

            angRot.x = Mathf.Clamp(angRot.x, jointInfo.angularLimits[0][0], jointInfo.angularLimits[0][1]);
            angRot.y = Mathf.Clamp(angRot.y, jointInfo.angularLimits[1][0], jointInfo.angularLimits[1][1]);
            angRot.z = Mathf.Clamp(angRot.z, jointInfo.angularLimits[2][0], jointInfo.angularLimits[2][1]);

            jointInfo.setConfigurableRot(angRot);
        }
    }

    private void applyVel(List<float> actions)
    {
        var actionIdx = 0;
        var size = actions.Count;

        if (size != actionsSpace)
        {
            Debug.Log("Wrong actions size: " + size + " != " + actionsSpace);
            return;
        }

        foreach (JointInfo jointInfo in jointInfos)
        {
            bool[] movableAxis = jointInfo.movableAxis;

            if (!movableAxis.Contains(true))
                continue;

            Vector3 angVel = new Vector3(0, 0, 0);
            if (movableAxis[0])
            {
                angVel.x = actions[actionIdx++] * jointInfo.maxVel;
            }

            if (movableAxis[1])
                angVel.y = actions[actionIdx++] * jointInfo.maxVel;
            if (movableAxis[2])
                angVel.z = actions[actionIdx++] * jointInfo.maxVel;

            //jointInfo.setConfigurableRotVel(angVel);
        }
    }

    public virtual int getActionsSpace()
    {
        var actions = 0;
        foreach (JointInfo jointInfo in bodyParts.jointsInfos)
        {
            if (!jointInfo.movableAxis.Contains(true))
                continue;

            actions += jointInfo.movableAxis.Count(b => b);
        }

        return actions;
    }

    protected float getEuqlides(float action, float[] limits)
    {
        float rot = Mathf.Lerp(limits[0], limits[1], (action + 1f) * .5f);
        return rot;
        //return action < 0 ? -action * limits[0] : action * limits[1];
        //return ((limits[1] - limits[0]) * ((action + 1f) / 2f))+ limits[0];
    }
}