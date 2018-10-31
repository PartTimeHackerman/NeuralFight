using System.Collections.Generic;
using UnityEngine;

public class TwoHandedAim : TwoHandedAction
{
    private Aim rAim;
    private Aim lAim;
 

    protected override void Start()
    {
        base.Start();
        rAim = rUpperArm.gameObject.AddComponent<Aim>();
        lAim = lUpperArm.gameObject.AddComponent<Aim>();
        rAim.target = target;
        lAim.target = target;
    }

    protected override void TakeAction()
    {
        rAim.activate = activate;
        lAim.activate = activate;
        done = rAim.done || lAim.done;
    }
}