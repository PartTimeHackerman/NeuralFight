using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    public float MaxHP;
    public float MaxSP;

    public float HpRegen;
    public float SpRegen;

    public float healthPoints;

    public float HealthPoints
    {
        get { return healthPoints; }

        set { HPSetter(value); }
    }

    public FighterPart fighterPart;
    public FighterPart FighterPart
    {
        get { return fighterPart; }
        set
        {
            fighterPart = value;
            FighterPart.Equip(this);
        }
    }

    public event ChangeHealth OnChangeHealth;

    public delegate void ChangeHealth(float newHP, float oldHP, float diffHP);
/*

    public float staminaPoints;

    public float StaminaPoints
    {
        get { return staminaPoints; }

        set { SPSetter(value); }
    }

    public event ChangeStamina OnChangeStamina;

    public delegate void ChangeStamina(float newSP, float oldSP, float diffSP);

*/

    public bool partEnabled = true;
    public bool partDetached = false;

    public List<Hitbox> Hitboxes = new List<Hitbox>();
    public List<BodyPart> childrensBodyParts;
    public Rigidbody Rigidbody;
    private ConfigurableJoint Joint;
    public JointInfo JointInfo;

    public Player Player;
    public bool setHp;
    
    public List<Rigidbody> Rigidbodies = new List<Rigidbody>();
    public MaterialChangerManager MaterialChangerManager;

    private void Start()
    {
        childrensBodyParts = GetComponentsInChildren<BodyPart>().ToList();
        childrensBodyParts.Remove(this);
        Joint = GetComponent<ConfigurableJoint>();
        JointInfo = GetComponent<JointInfo>();
        foreach (Hitbox hitbox in Hitboxes)
        {
            hitbox.BodyPart = this;
        }

        HealthPoints = MaxHP;
        Rigidbody = GetComponent<Rigidbody>();
        //StaminaPoints = MaxSP;
        //StartCoroutine(RegenerateHpSp());
    }

    private void HPSetter(float value)
    {
        if (Math.Abs(healthPoints - value) < .0001f) return;
        if (value >= MaxHP) value = MaxHP;
        //if (value < 0) value = 0f;
        //if (value >= 0)
        //{
        float diffrence = healthPoints - value; //value < 0 ? healthPoints : healthPoints - value;
        OnChangeHealth?.Invoke(value, healthPoints, diffrence);
        //}

        healthPoints = value;
        if (healthPoints <= 0 && partEnabled) Disable();
        //if (healthPoints <= -MaxHP && !partDetached) Detach();
    }
/*

    private void SPSetter(float value)
    {
        if (Math.Abs(staminaPoints - value) < .001f || !partEnabled) return;
        if (value > MaxSP) value = MaxSP;
        OnChangeHealth?.Invoke(value, staminaPoints, staminaPoints - value);
        staminaPoints = value;
    }
*/

    public void Disable()
    {
        if (gameObject.name.Equals("butt")) return;
        Player.DisableBodyPart(this);
        partEnabled = false;
        if (JointInfo != null)
        {
            if (JointInfo.enabled) JointInfo.Disable();
        }

        /*
         foreach (BodyPart bodyPart in childrensBodyParts)
        {
            bodyPart.DisableChildren();
        }
        */
    }

    public void Enable()
    {
        partEnabled = true;
        if (JointInfo != null)
        {
            JointInfo.Enable();
        }

        /*
         foreach (BodyPart bodyPart in childrensBodyParts)
        {
            bodyPart.DisableChildren();
        }
        */
    }

    public void ResetPart()
    {
        healthPoints = MaxHP;
        Enable();
    }

    public void Detach()
    {
        if (gameObject.name.Equals("butt")) return;
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            child.tag = "detached";
            GameObject childGO = child.gameObject;
            childGO.layer = LayerMask.NameToLayer("Detached");
            WeaponHolder weaponHolder = child.GetComponent<WeaponHolder>();
            if (weaponHolder != null)
            {
                GameObject weaponGO = weaponHolder.ArmWeapon.Weapon.Rigidbody.gameObject;
                weaponGO.layer = LayerMask.NameToLayer("Detached");
                StartCoroutine(Waiter.WaitForSeconds(1, () => { }, () =>
                {
                    weaponGO.layer = LayerMask.NameToLayer("Default");
                    childGO.layer = LayerMask.NameToLayer("Default");
                }));
                weaponHolder.ArmWeapon.Weapon = null;
            }
        }

        gameObject.transform.parent = null;
        if (HealthPoints <= 0)
        {
            DisableJoints();
        }

        partDetached = true;
    }

    /*public void DisableChildren()
    {
        Player.DisableBodyPart(this);
        partEnabled = false;
        if (JointInfo != null)
        {
            if (JointInfo.enabled) JointInfo.Disable();
        }
    }*/

    public void DisableJoints()
    {
        if (Joint != null)
            Destroy(Joint);
    }

    private IEnumerator RegenerateHpSp()
    {
        while (true)
        {
            if (partEnabled)
            {
                HealthPoints += HpRegen / 10f;
            }

            yield return new WaitForSeconds(.1f);
        }
    }
}