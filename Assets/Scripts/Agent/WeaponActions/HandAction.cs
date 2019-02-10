using System;
using UnityEngine;

public class HandAction : MonoBehaviour
{
    public Transform target;

    public Transform Target
    {
        get { return target; }
        set
        {
            target = value;
            if (attack != null)
            {
                attack.target = target;
            }

            if (block != null)
            {
                block.target = target;
            }
        }
    }

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

    private Transform TorsoTransform;
    public float baseDist;

    public int AttackDelayFrames = 0;

    public virtual void Start()
    {
        TorsoTransform = Player.Body.GetComponent<BodyParts>().getNamedParts()["torso"].transform;
        Transform hand = Player.Body.GetComponent<BodyParts>().getNamedParts()["rhand_end"].transform;
        baseDist = Vector3.Distance(TorsoTransform.position, hand.position);
    }

    void FixedUpdate()
    {
        if (active && used)
        {
            block.activate = !attack.activate;

            if (attack.activate)
            {
                weapon.DamagingPart.damage = weapon.Damage;
            }
            else
            {
                weapon.DamagingPart.damage = weapon.Damage / 10f;
                Vector3 torsoPos = TorsoTransform.position;
                torsoPos.z = 0f;
                Vector3 targetPos = target.position;
                targetPos.z = 0f;
                float distance = Vector3.Distance(torsoPos, targetPos);
                //if (Math.Abs(Mathf.Clamp(distance, weapon.MinAttackDistance, weapon.MaxAttackDistance) - distance) < .001f)
                //{
                if (distance >= weapon.MinAttackDistance && distance <= weapon.MaxAttackDistance)
                {
                    Waiter.Get().WaitForFramesC(AttackDelayFrames, () => { }, Attack);
                    //Attack();
                }
            }
        }

        if (attackWep)
        {
            Attack();
            attackWep = false;
        }
    }

    public void SetAttackBlockActions(Type attack, Type block)
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
        //StartCoroutine(Waiter.WaitForFrames(10, () => { }, () => { setActive(true); }));
    }

    public void Attack()
    {
        if (canBeUsed && active)
        {
            OnActivate?.Invoke(this, SPReq);
            attack.activate = true;
        }
    }

    public void setActive(bool active)
    {
        this.active = active;
        weapon.SetDamaging(active);
    }

    public void setCanBeUsed(float SP)
    {
        canBeUsed = SP >= SPReq;
    }

    public void unEquipAction()
    {
        if (this.attack != null) Destroy(this.attack);
        if (this.block != null) Destroy(this.block);
        //active = false;
    }

    public virtual void EquipAction(Weapon newWeapon, Weapon oldWeapon)
    {
        unEquipAction();
        if (newWeapon != null)
        {
            weapon = newWeapon;
            SPReq = weapon.SPReq;
            SetAttackBlockActions(getAttackAction(weapon), getBlockAction(weapon));
        }
    }

    public Type getAttackAction(Weapon weapon)
    {
        switch (weapon.WeaponAttack)
        {
            case WeaponAttack.PUSH:
                //if (weapon.WeaponHand == WeaponHand.BOTH)
                //    return typeof(TwoHandedStab);
                //else
                return typeof(Stab);
                break;
            case WeaponAttack.SLASH:
                //if (weapon.WeaponHand == WeaponHand.BOTH)
                //    return typeof(TwoHandedSlash);
                //else
                return typeof(Slash);
                break;
            case WeaponAttack.AIM:
                //if (weapon.WeaponHand == WeaponHand.BOTH)
                //    return typeof(TwoHandedAim);
                //else
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