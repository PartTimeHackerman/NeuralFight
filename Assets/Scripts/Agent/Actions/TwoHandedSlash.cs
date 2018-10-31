using System.Collections.Generic;
using UnityEngine;

public class TwoHandedSlash : TwoHandedAction
{
    private Slash rSlash;
    private Slash lSlash;

    public float upperArmReadyAng = -160f;
    public float lowerArmReadyAng = 45f;
    public float upperArmSlashAng = 0f;
    public float lowerArmSlashAng = 20f;

    protected override void Start()
    {
        base.Start();
        rSlash = rUpperArm.gameObject.AddComponent<Slash>();
        lSlash = lUpperArm.gameObject.AddComponent<Slash>();
        setSlash(rSlash);
        setSlash(lSlash);
    }

    protected override void TakeAction()
    {
        rSlash.activate = activate;
        lSlash.activate = activate;
        done = rSlash.done || lSlash.done;
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