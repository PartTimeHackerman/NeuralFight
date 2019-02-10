using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionsAngPosStrength : ActionsAngPos
{
    protected override void ApplyPos(List<float> actions)
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

            float force = ((actions[actionIdx++] + 1f) * .5f);

            Vector3 angRot = new Vector3(0, 0, 0);
            if (movableAxis[0])
                angRot.x = getEuqlides(actions[actionIdx++], jointInfo.angularLimits[0]);
            if (movableAxis[1])
                angRot.y = getEuqlides(actions[actionIdx++], jointInfo.angularLimits[1]);
            if (movableAxis[2])
                angRot.z = getEuqlides(actions[actionIdx++], jointInfo.angularLimits[2]);

            //angRot.x = Mathf.Clamp(angRot.x, jointInfo.angularLimits[0][0], jointInfo.angularLimits[0][1]);
            //angRot.y = Mathf.Clamp(angRot.y, jointInfo.angularLimits[1][0], jointInfo.angularLimits[1][1]);
            //angRot.z = Mathf.Clamp(angRot.z, jointInfo.angularLimits[2][0], jointInfo.angularLimits[2][1]);

            jointInfo.SetConfigurableForceAndRot(force, angRot);
        }
    }


    public override int getActionsSpace()
    {
        var actions = 0;
        foreach (JointInfo jointInfo in bodyParts.jointsInfos)
        {
            if (!jointInfo.movableAxis.Contains(true) || !jointInfo.useNeural)
                continue;

            actions += jointInfo.movableAxis.Count(b => b);
            actions += 1;
        }

        return actions;
    }
}