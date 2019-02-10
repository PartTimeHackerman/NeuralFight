using UnityEngine;

public class TwoHandAction : HandAction
{
    public BodyParts LeftBodyParts;
    public BodyParts RightBodyParts;

    public override void EquipAction(Weapon newWeapon, Weapon oldWeapon)
    {
        if (newWeapon.WeaponAttack == WeaponAttack.AIM)
        {
            BodyParts = LeftBodyParts;
            foreach (JointInfo jointInfo in RightBodyParts.jointsInfos)
            {
                jointInfo.Disable();
            }
        }
        else
        {
            BodyParts = RightBodyParts;
            foreach (JointInfo jointInfo in LeftBodyParts.jointsInfos)
            {
                jointInfo.Disable();
            }
            Debug.Log("r");
        }
        base.EquipAction(newWeapon, oldWeapon);
    }
}