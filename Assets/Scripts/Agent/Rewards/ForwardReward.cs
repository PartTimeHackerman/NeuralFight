using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


class ForwardReward
{
    private BodyParts bodyParts;
    private BodyParts enemyBodyParts;
    private PhysicsUtils physics;
    private Vector3 COMOld = new Vector3();
    private Vector3 COM;
    private Vector3 COMVel;
    public float reward;

    public ForwardReward(BodyParts bodyParts, BodyParts enemyBodyParts)
    {
        this.bodyParts = bodyParts;
        this.enemyBodyParts = enemyBodyParts;
        physics = PhysicsUtils.get();
    }

    public float getReward()
    {
        COMVel = physics.getCenterOfMassVel(bodyParts.getRigids());

        //Vector3 rotateV = Vector3.RotateTowards(COM, enemnyCOM, 1, 1);
        //float velRew = COMVel.x * rotateV.x + COMVel.y * rotateV.y;

        float XDiff = Mathf.Abs(COMOld.x) + COM.x;
        XDiff = Mathf.Clamp(XDiff, -1f, 1f);
        COMOld = COM;
        float XVel = Mathf.Clamp(COMVel.x, -10f, 10f) / 10;
        reward = XVel;
        if (COM.x > 0)
            reward = -reward;
        
        return reward;

    }

}
