using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FightObservationsForEnemy : MonoBehaviour
{
    public Player Player;
    public HandAction RightHandAction;
    public HandAction LeftHandAction;
    public BodyParts BodyParts;
    public List<float> observations = new List<float>();
    private PhysicsUtils physics;

    void Start()
    {
        //observationsSpace = GetObservations(true).Count();
        physics = PhysicsUtils.get();
    }

    public List<float> GetObservations(bool asLeft)
    {
        observations.Clear();
        AddHpSp();
        AddActions();
        AddParts(asLeft);
        return observations;
    }

    private void AddHpSp()
    {
        observations.Add(Player.hp / Player.MaxHP);
        observations.Add(Player.sp / Player.MaxSP);
    }

    private void AddActions()
    {
        observations.Add(RightHandAction.canBeUsed ? 1f : 0f);
        observations.Add(RightHandAction.SPReq / Player.sp);
        if (RightHandAction.weapon == null)
        {
            observations.Add(0f);
            observations.Add(0f);
        }
        else
        {
            observations.Add(RightHandAction.weapon.WeaponAttack == WeaponAttack.SLASH ? 1f : 0f);
            observations.Add(RightHandAction.weapon.WeaponBlock == WeaponBlock.BLOCK ? 1f : 0f);
        }
        
        observations.Add(LeftHandAction.canBeUsed ? 1f : 0f);
        observations.Add(LeftHandAction.SPReq / Player.sp);
        if (LeftHandAction.weapon == null)
        {
            observations.Add(0f);
            observations.Add(0f);
        }
        else
        {
            observations.Add(LeftHandAction.weapon.WeaponAttack == WeaponAttack.SLASH ? 1f : 0f);
            observations.Add(LeftHandAction.weapon.WeaponBlock == WeaponBlock.BLOCK ? 1f : 0f);
        }
    }

    private void AddParts(bool asLeft)
    {
        observations.Add(asLeft ? BodyParts.root.position.x / 10f : -BodyParts.root.position.x / 10f);
        observations.Add(Observations.distToFloor(BodyParts.root.transform));
        Quaternion quaternion = BodyParts.transform.rotation;
        Vector3 rotEul = quaternion.eulerAngles;
        float rotAng = rotEul.z;
        float rotClamped = 0f;
        if (rotAng <= 180f)
            rotClamped = rotAng / 180f;
        else
            rotClamped = ((rotAng - 180f) / 180f) - 1f;
        observations.Add(asLeft ? rotClamped : -rotClamped);
        
        foreach (Transform ending in BodyParts.endings)
        {
            Vector3 endRelPos = Observations.getObjectPosition(ending, BodyParts.root);
            observations.Add(endRelPos.x);
            observations.Add(endRelPos.y);
            observations.Add(Observations.distToFloor(ending));
        }
        
        Vector3 COM;
        Vector3 COMVel;
        Vector3 COMAngVel;
        physics.getCenterOfMassAll(BodyParts.getRigids(), out COM, out COMVel, out COMAngVel);
        COM = BodyParts.root.transform.InverseTransformPoint(COM);
        COMAngVel = Quaternion.Inverse(BodyParts.root.rotation) * COMAngVel;
        observations.Add(COM.x);
        observations.Add(COM.y);
        observations.Add(asLeft ? COMVel.x : -COMVel.x);
        observations.Add(COMVel.y);
        observations.Add(COMAngVel.z);
    }
}