using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float MaxHP = 0f;
    public float MaxSP = 0f;
    public float CurrentMaxHP = 0f;
    public float CurrentMaxSP = 0f;
    public float hp = 0f;

    public float HP
    {
        get { return hp; }
        set { HPSetter(value); }
    }

    public float sp = 0f;

    public float HpRegen;
    public float SpRegen;

    public float SP
    {
        get { return sp; }

        set { SPSetter(value); }
    }

    public GameObject Body;
    public bool died = false;

    public ActionsContainer ActionsContainer;

    private List<BodyPart> Parts;
    private Dictionary<BodyPart, float> StaminaPrecentages = new Dictionary<BodyPart, float>();
    private BodyParts BodyParts;
    public Observations BodyObservations;
    public WeaponEquipper WeaponEquipper;
    public bool left = true;
    
    public event ChangeHealth OnChangeHealth;
    public delegate void ChangeHealth(float currentHP, float maxHP);
    
    public event ChangeStamina OnChangeStamina;
    public delegate void ChangeStamina(float currentSP, float maxSP);

    public HitNumber HitNumberPref;
    public HitNumber HitNumber;
    

    private void Start()
    {
        Parts = Body.GetComponentsInChildren<BodyPart>().ToList();
        BodyParts = Body.GetComponent<BodyParts>();
        HitNumber = Instantiate(HitNumberPref);
        HitNumber.Player1 = !left;
        HitNumber.setParent(BodyParts.getNamedParts()["head"].transform);
        ResetPlayer();
        foreach (BodyPart bodyPart in Parts)
        {
            bodyPart.OnChangeHealth += ChangeHp;
        }
        StartCoroutine(RegenerateHpSp());
    }

    public void ResetPlayer()
    {
        died = false;
        MaxHP = 0;
        MaxSP = 0;
        HpRegen = 0;
        SpRegen = 0;
        foreach (BodyPart bodyPart in Parts)
        {
            bodyPart.Enable();
            bodyPart.Player = this;
            MaxHP += bodyPart.MaxHP;
            MaxSP += bodyPart.MaxSP;
            HpRegen += bodyPart.HpRegen;
            SpRegen += bodyPart.SpRegen;
        }

        CurrentMaxHP = MaxHP;
        CurrentMaxSP = MaxSP;
        HP = MaxHP;
        SP = MaxHP;
        OnChangeHealth?.Invoke(this.hp, MaxHP);
        OnChangeStamina?.Invoke(sp, MaxSP);
    }

    private void FixedUpdate()
    {
        hp = hp > CurrentMaxHP ? CurrentMaxHP : hp;
        sp = sp > CurrentMaxSP ? CurrentMaxSP : sp;
        ActionsContainer.rightHand.setCanBeUsed(sp);
        ActionsContainer.leftHand.setCanBeUsed(sp);
        //ActionsContainer.bothHands.setCanBeUsed(sp);
    }

    private void ChangeHp(float hp, float oldHp, float diffHp)
    {
        this.hp = this.hp - diffHp < 0f ? 0f : this.hp - diffHp;
        this.hp = this.hp < CurrentMaxHP ? this.hp : CurrentMaxHP;
        OnChangeHealth?.Invoke(this.hp, MaxHP);
        HitNumber.Anim(diffHp);
        if (this.hp <= 0)
        {
            OnDie();
        }
    }

    private void ChangeSP(float sp, float oldSp, float diffSp)
    {
        this.sp = this.sp - diffSp < 0f ? 0f : this.sp - diffSp;
        this.sp = this.sp < CurrentMaxSP ? this.sp : CurrentMaxSP;
    }

    private void HPSetter(float value)
    {
        if (Math.Abs(hp - value) < .001f) return;
        if (value > CurrentMaxHP) value = CurrentMaxHP;
        if (value < 0) value = 0f;
        hp = value;
    }

    private void SPSetter(float value)
    {
        if (Math.Abs(sp - value) < .001f) return;
        if (value > CurrentMaxSP) value = CurrentMaxSP;
        if (value < 0) value = 0;
        sp = value;
        OnChangeStamina?.Invoke(sp, MaxSP);
        ActionsContainer.rightHand.setCanBeUsed(sp);
        ActionsContainer.leftHand.setCanBeUsed(sp);
        //ActionsContainer.bothHands.setCanBeUsed(sp);
    }

    public void DisableBodyPart(BodyPart bodyPart)
    {
        //CurrentMaxHP -= bodyPart.MaxHP;
        //CurrentMaxSP -= bodyPart.MaxSP;
        HpRegen -= bodyPart.HpRegen;
        SpRegen -= bodyPart.SpRegen;
    }

    private void OnDie()
    {
        died = true;
        foreach (JointInfo jointsInfo in BodyParts.jointsInfos)
        {
            jointsInfo.Disable();
        }
    }

    private IEnumerator RegenerateHpSp()
    {
        while (true)
        {
            if (!died)
            {
                SP += SpRegen / 10f;
                HP += HpRegen / 10f;
            }
            yield return new WaitForSeconds(.1f);
        }
    }

    private void SetWaitersActive()
    {
        foreach (IWaiter waiter in GetComponentsInChildren<IWaiter>())
        {
            waiter.setActive(true);
        }
    }

    public void ActivateWeaponAction(HandAction handaction, float sp)
    {
        SP -= sp;
    }
}