using System.Collections.Generic;
using UnityEngine;

public class TwoHandedSlash : TwoHandedAction<Slash>
{
    public float upperArmReadyAng = -160f;
    public float lowerArmReadyAng = 45f;
    public float upperArmSlashAng = 0f;
    public float lowerArmSlashAng = 20f;

    public override void setUpAction(BodyParts bodyParts)
    {
        setAction = () =>
        {
            setSlash(rArmAction);
            setSlash(lArmAction);
        };

        base.setUpAction(bodyParts);
    }

    private void setSlash(Slash slash)
    {
        slash.target = target;
        slash.upperArmReadyAng = upperArmReadyAng;
        slash.lowerArmReadyAng = lowerArmReadyAng;
        slash.upperArmSlashAng = upperArmSlashAng;
        slash.lowerArmSlashAng = lowerArmSlashAng;
    }
}