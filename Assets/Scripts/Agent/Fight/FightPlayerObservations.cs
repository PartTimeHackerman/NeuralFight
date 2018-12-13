using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FightPlayerObservations : MonoBehaviour
{
    private Player Player;
    private HandAction RightHandAction;
    private HandAction LeftHandAction;
    private BodyParts BodyParts;
    public BodyParts EnemyBodyParts;
    public FightObservationsForEnemy enemyObservations;
    public List<float> observations = new List<float>();
    public int observationsSpace;
    private PhysicsUtils physics;

    void Start()
    {
        //observationsSpace = GetObservations(true).Count();
        physics = PhysicsUtils.get();
    }

    public void SetUp(FightPlayerAgent player, FightPlayerAgent enemy)
    {
        Player = player.PlayerFighter.Player;
        RightHandAction = player.PlayerFighter.RightArmWeapon.HandAction;
        LeftHandAction = player.PlayerFighter.LeftArmWeapon.HandAction;
        BodyParts = player.PlayerFighter.BodyParts;
        EnemyBodyParts = enemy.PlayerFighter.BodyParts;
    }

    public List<float> GetObservations(bool asLeft)
    {
        observations.Clear();
        observations.Add(GameTimer.get().Elapsed / 60f);
        AddHpSp();
        //AddActions();
        AddParts(asLeft);
        observations.AddRange(enemyObservations.GetObservations(asLeft));
        removeInfsAndNans(observations);
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
        
        observations.Add(Vector3.Distance(BodyParts.root.position, EnemyBodyParts.root.position));
    }
    
    private void removeInfsAndNans(List<float> obs)
    {
        for (int i = obs.Count - 1; i >= 0; i--)
        {
            float v = obs[i];
            if (float.IsNaN(v) || float.IsInfinity(v) || float.IsNegativeInfinity(v) || float.IsPositiveInfinity(v))
            {
                obs[i] = 0f;
            }
        }
    }
}