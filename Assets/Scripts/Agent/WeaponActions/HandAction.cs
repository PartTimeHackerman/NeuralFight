using System;
using UnityEngine;

public class HandAction : MonoBehaviour, IWaiter
{
    public Transform target;
    public float SPReq;
    public BodyParts BodyParts;
    public WeaponAction attack;
    public WeaponAction block;
    public Weapon weapon;
    public bool RequireSP = true;

    public bool canBeUsed = false;
    public bool active = false;
    private bool used = false;
    public Player Player;
    
    public event Activate OnActivate;
    public delegate void Activate(HandAction handAction, float sp);

    public bool attackWep = false;

    public virtual void Start()
    {
    }

    void FixedUpdate()
    {
        if (active && used)
        {
            block.activate = !attack.activate;
        }

        if (attackWep)
        {
            Attack();
            attackWep = false;
        }
    }

    public void setAttackBlockActions(Type attack, Type block)
    {
        used = true;
        this.attack = (WeaponAction) gameObject.AddComponent(attack);
        this.block = (WeaponAction) gameObject.AddComponent(block);

        this.attack.setUpAction(BodyParts);
        this.block.setUpAction(BodyParts);

        this.attack.target = target;
        this.block.target = target;

        this.block.deactivateOnDone = false;
        OnActivate += Player.ActivateWeaponAction;
        StartCoroutine(Waiter.WaitForFrames(10, () => { }, () => { setActive(true); }));
    }

    public void Attack()
    {
        if (canBeUsed)
        {
            OnActivate?.Invoke(this, SPReq);
            attack.activate = true;
        }
    }

    public void setActive(bool active)
    {
        if (canBeUsed)
        {
            this.active = active;
        }
    }

    public void setCanBeUsed(float SP)
    {
        canBeUsed = SP >= SPReq;
    }

    public void unEquipAction()
    {
        if (this.attack != null) Destroy(this.attack);
        if (this.block != null) Destroy(this.block);
        active = false;
    }

    public void equipAction(Weapon newWeapon, Weapon oldWeapon)
    {
        unEquipAction();
        if (newWeapon != null)
        {
            weapon = newWeapon;
            SPReq = weapon.SPReq;
            setAttackBlockActions(getAttackAction(weapon), getBlockAction(weapon));
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
}