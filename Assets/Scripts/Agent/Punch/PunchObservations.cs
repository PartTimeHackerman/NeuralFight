using System.Collections.Generic;
using UnityEngine;

public class PunchObservations : Observations
{
    public Transform punchTarget;
    
    public override Dictionary<string, float> getObservationsNamed()
    {
        Dictionary<string, float> obsNamed = base.getObservationsNamed();
        obsNamed["ref_target_rot"] = endPointRot();
        return obsNamed;
    }

    float endPointRot()
    {
        Vector3 relativePos = punchTarget.position - root.position;
        float rotAng = Vector3.Angle(relativePos, root.transform.right);
        float rotClamped = 0f;
        if (rotAng <= 180f)
            rotClamped = rotAng / 180f;
        else
            rotClamped = ((rotAng - 180f) / 180f) - 1f;

        if (relativePos.y > 0)
        {
            rotClamped *= -1f;
        }

        return rotClamped;
    }
}