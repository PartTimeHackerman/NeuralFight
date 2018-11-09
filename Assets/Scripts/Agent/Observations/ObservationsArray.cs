using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObservationsArray : Observations
{
    private float[] obs;
    private int i = 0;
    public bool left = true;

    private int gather = 0;

    protected override void Start()
    {
        observationsSpace = 54;
        obs = new float[observationsSpace];
    }

    public override void setObjectPosition(Transform transform)
    {
        Vector3 pos = getObjectPosition(transform, root);
        obs[i++] = pos.x;
        obs[i++] = pos.y;
    }

    public override void setObjectRotation(Rigidbody rigidbody)
    {
        float rot = getObjectRotation(rigidbody, root);
        obs[i++] = rot;
    }

    public override void setObjectVel(Rigidbody rigidbody)
    {
        Vector3 vel = getObjectVel(rigidbody);
        obs[i++] = left ? vel.x : -vel.x;
        obs[i++] = vel.y;
    }

    public override void setObjectAngVel(Rigidbody rigidbody)
    {
        obs[i++] = left ? getObjectAngVel(rigidbody):-getObjectAngVel(rigidbody);
    }

    public override void getEndingsGroundDist()
    {
        foreach (var ending in bodyParts.endings)
        {
            obs[i++] = distToFloor(ending);
        }
    }

    public override List<float> getObservations()
    {
        i = 0;
        var rootPos = root.transform.position;
        //obs[i++] = normPos(rootPos.x);
        obs[i++] = distToFloor(root.transform);
        Quaternion quaternion = root.transform.rotation;
        Vector3 rotEul = quaternion.eulerAngles;
        float rotAng = rotEul.z;
        float rotClamped = 0f;
        if (rotAng <= 180f)
            rotClamped = rotAng / 180f;
        else
            rotClamped = ((rotAng - 180f) / 180f) - 1f;
        
        obs[i++] = left ? rotClamped : -rotClamped;


        getObjPosRotVelAngVel();

        addCOM();

        getEndingsGroundDist();

        return obs.ToList();
    }

    public override void addCOM()
    {
        Vector3 COM;
        Vector3 COMVel;
        Vector3 COMAngVel;
        physics.getCenterOfMassAll(rigids, out COM, out COMVel, out COMAngVel);
        COM = root.transform.InverseTransformPoint(COM);
        COMAngVel = Quaternion.Inverse(root.rotation) * COMAngVel;
        obs[i++] = COM.x;
        obs[i++] = COM.y;
        obs[i++] = left ? COMVel.x :-COMVel.x;
        obs[i++] = COMVel.y;
        obs[i++] = COMAngVel.z;
    }
}