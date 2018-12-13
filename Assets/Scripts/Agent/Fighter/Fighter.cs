using System.Collections.Generic;
using UnityEngine;


public class Fighter : MonoBehaviour
{
    public BodyParts BodyParts;
    public ArmWeapon RightArmWeapon;
    public ArmWeapon LeftArmWeapon;
    public FighterNum FighterNum;
    public FightAction FightAction;
    public Player Player;
    public List<LocomotionAction> LocomotionActions = new List<LocomotionAction>();
    public List<VerticalEffector> VerticalEffectors = new List<VerticalEffector>();
    public List<VelocityEffector> VelocityEffectors = new List<VelocityEffector>();
    public ObservationsArray ObservationsArray;
    public bool Left;

    public FighterDefaultPositioner FighterDefaultPositioner;

    public void StartFight()
    {
        BodyParts.setKinematic(false);
        RightArmWeapon.HandAction.setActive(true);
        LeftArmWeapon.HandAction.setActive(true);
        FightAction.run = true;
        SetEnableFighter(true);
    }
    
    public void StopFight()
    {
        RightArmWeapon.HandAction.setActive(false);
        LeftArmWeapon.HandAction.setActive(false);
        FightAction.run = false;
        SetEnableFighter(false);
        LocomotionActions.ForEach(a => a.run = false);
    }

    public void SetEnableFighter(bool enable)
    {
        BodyParts.SetEnableJoints(enable);
    }

    public void ResetFighter()
    {
        StopFight();
        FighterDefaultPositioner.ResetPosition();
        transform.position = Player.left ? ObjectsPositions.PlayerFightersPos : ObjectsPositions.EnemyFightersPos;
        BodyParts.setKinematic(true);
    }

    public void SetSide(bool left)
    {
        Player.left = left;
        VerticalEffectors.ForEach(e => e.left = left);
        VelocityEffectors.ForEach(e => e.left = left);
        ObservationsArray.left = left;
    }

    public void SetEnemy(Fighter enemy)
    {
        RightArmWeapon.HandAction.Target = enemy.BodyParts.getNamedParts()["torso"].transform;
        LeftArmWeapon.HandAction.Target = enemy.BodyParts.getNamedParts()["torso"].transform;
        FightAction.SetEnemy(enemy);
    }
}