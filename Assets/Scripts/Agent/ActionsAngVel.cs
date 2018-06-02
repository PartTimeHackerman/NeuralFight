using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionsAngVel : MonoBehaviour, IActions
{
    public int actionsSpace;

    private BodyParts bodyParts;
    private List<JointInfo> jointInfos;

    private void Start()
    {
        bodyParts = GetComponent<BodyParts>();
        actionsSpace = getActionsSpace();
        jointInfos = bodyParts.jointsInfos;
    }


    public void applyActions(List<float> actions)
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
                angVel.x = Mathf.Clamp(actions[actionIdx++] * jointInfo.maxPosSpring, -1200, 1200);
            if (movableAxis[1])
                angVel.y = Mathf.Clamp(actions[actionIdx++] * jointInfo.maxPosSpring, -12000, 1200);
            if (movableAxis[2])
                angVel.z = Mathf.Clamp(actions[actionIdx++] * jointInfo.maxPosSpring, -1200, 1200);

            float sum = Mathf.Abs(angVel.x) + Mathf.Abs(angVel.y) + Mathf.Abs(angVel.y);
            angVel.x *= Mathf.Abs(angVel.x) / sum;
            angVel.y *= Mathf.Abs(angVel.y) / sum;
            angVel.z *= Mathf.Abs(angVel.z) / sum;

            //if (jointInfo.conAxis >= 0) angVel[jointInfo.conAxis] /= 10;
            jointInfo.setConfigurableRotVel(angVel);

        }
    }

    public int getActionsSpace()
    {
        var actions = 0;
        foreach (JointInfo jointInfo in bodyParts.jointsInfos)
        {
            actions += jointInfo.movableAxis.Count(b => b);
        }
        return actions;
    }

    private float getEuqlides(float action, float[] limits)
    {
        return action < 0 ? -action * limits[0] : action * limits[1];
    }
}