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
        COM = physics.getCenterOfMass(bodyParts.getRigids());
        COMVel = physics.getCenterOfMassVel(bodyParts.getRigids());
        COMVel.z = 0;
        float COMVelMag = COMVel.sqrMagnitude;
        COMVel += COM;
        Vector3 eCOM = physics.getCenterOfMass(enemyBodyParts.getRigids());
        Vector2 vec2 = new Vector2(eCOM.x, eCOM.y);
        Vector2 vec1 = new Vector2(COMVel.x, COMVel.y);
        Vector2 vec3 = new Vector2(COM.x, COM.y);
        float velAng = Vector2.Angle(vec1 - vec3, vec2 - vec3);
        velAng = Mathf.Abs((velAng / 180f) - 1f);
        reward = velAng + velAng  * COMVelMag / 10;
        return reward;

    }

}
