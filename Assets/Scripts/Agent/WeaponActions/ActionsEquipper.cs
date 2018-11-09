using System;
using UnityEngine;

public class ActionsEquipper : MonoBehaviour
{
    public ActionsContainer ActionsContainer;
    public WeaponsContainer WeaponsContainer;
    
/*

    public void equipActions()
    {
        foreach (Weapon weapon in WeaponsContainer.weapons)
        {
            equipAction(weapon);
        }

    }

    private void equipAction(Weapon weapon)
    {
        switch (weapon.WeaponHand)
        {
            case WeaponHand.RIGHT:
                ActionsContainer.rightHand.setAttackBlockActions(getAttackAction(weapon), getBlockAction(weapon));
                break;
            case WeaponHand.LEFT:
                ActionsContainer.leftHand.setAttackBlockActions(getAttackAction(weapon), getBlockAction(weapon));
                break;
            case WeaponHand.BOTH:
                ActionsContainer.bothHands.setAttackBlockActions(getAttackAction(weapon), getBlockAction(weapon));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    public Type getAttackAction(Weapon weapon)
    {
        switch (weapon.WeaponAttack)
        {
            case WeaponAttack.PUSH:
                if (weapon.WeaponHand == WeaponHand.BOTH)
                    return typeof(TwoHandedStab);
                else
                    return typeof(Stab);
                break;
            case WeaponAttack.SLASH:
                if (weapon.WeaponHand == WeaponHand.BOTH)
                    return typeof(TwoHandedSlash);
                else
                    return typeof(Slash);
                break;
            case WeaponAttack.AIM:
                if (weapon.WeaponHand == WeaponHand.BOTH)
                    return typeof(TwoHandedAim);
                else
                    return typeof(Aim);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public Type getBlockAction(Weapon weapon)
    {
        switch (weapon.WeaponBlock)
        {
            case WeaponBlock.BLOCK:
                return typeof(Block);
                break;
            case WeaponBlock.LONG_BLOCK:
                return typeof(LongBlock);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    */

}
