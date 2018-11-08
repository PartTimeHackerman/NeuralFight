using System;
using System.Collections.Generic;
using UnityEngine;

public class ArmWeapon : MonoBehaviour
{
    public Weapon weapon;

    public Weapon Weapon
    {
        get { return weapon; }

        set
        {
            if (weapon == value) return;
            OnChangeWeapon?.Invoke(value, weapon);
            weapon = value;
        }
    }

    public event ChangeWeapon OnChangeWeapon;
    public delegate void ChangeWeapon(Weapon newWeapon, Weapon oldWeapon);

    public WeaponHand WeaponHand;
    public BodyParts BodyParts;
    public HandAction HandAction;

    public Rigidbody rHandRb;
    public Rigidbody lHandRb;

    private ConfigurableJoint rJoint;
    private ConfigurableJoint lJoint;
    private bool unEquip = false;

    protected virtual void Start()
    {
        OnChangeWeapon += equipWeapon;
        OnChangeWeapon += HandAction.equipAction;
        foreach (KeyValuePair<string, Rigidbody> keyValuePair in BodyParts.getNamedRigids())
        {
            if (keyValuePair.Key.Contains("rhand")) rHandRb = keyValuePair.Value;
            if (keyValuePair.Key.Contains("lhand")) lHandRb = keyValuePair.Value;
        }
    }

    private void FixedUpdate()
    {
        if (unEquip)
        {
            unEquipWeapon(weapon);
        }
    }


    public void equipWeapon(Weapon newWeapon, Weapon oldWeapon)
    {
        unEquipWeapon(oldWeapon);
        if (newWeapon == null)
        {
            return;
        }
        weapon = newWeapon;
        weapon.transform.parent = gameObject.transform;
        Action first = () => { setWeaponPos(weapon); };
        Action second = () => { setWeaponJoints(weapon); };
        StartCoroutine(Waiter.WaitForFrames(3, first, second));
    }

    public void unEquipWeapon(Weapon oldWeapon)
    {
        if (oldWeapon != null)
        {
            oldWeapon.transform.parent = null;
            if (rJoint != null) Destroy(rJoint);
            if (lJoint != null) Destroy(lJoint);
            weapon = null;
        }
    }

    private void setWeaponPos(Weapon weapon)
    {
        Vector3 weaponPos = new Vector3();
        Quaternion weaponRot = new Quaternion();

        switch (WeaponHand)
        {
            case WeaponHand.RIGHT:
            {
                weaponPos = rHandRb.transform.position;
                weaponRot = rHandRb.rotation;
                break;
            }
            case WeaponHand.LEFT:
            {
                weaponPos = lHandRb.transform.position;
                weaponRot = lHandRb.rotation;
                break;
            }
            case WeaponHand.BOTH:
            {
                Vector3 rHandPosWep = rHandRb.transform.position;
                rHandPosWep.z = 0f;
                weaponPos = rHandPosWep;
                weaponRot = rHandRb.rotation;

                Vector3 lHandPosWep = rHandRb.transform.position;
                lHandPosWep.z = lHandRb.transform.position.z;
                lHandRb.position = lHandPosWep;
                break;
            }
        }

        weapon.grip.isKinematic = true;
        weapon.grip.position = weaponPos;
        Vector3 rot = weaponRot.eulerAngles;
        rot.z -= 90f;
        weapon.grip.rotation = Quaternion.Euler(rot);
    }


    private void setWeaponJoints(Weapon weapon)
    {
        switch (WeaponHand)
        {
            case WeaponHand.RIGHT:
            {
                rJoint = addConfigurableJoint(rHandRb, weapon.grip);
                break;
            }
            case WeaponHand.LEFT:
            {
                lJoint = addConfigurableJoint(lHandRb, weapon.grip);
                break;
            }
            case WeaponHand.BOTH:
            {
                rJoint = addConfigurableJoint(rHandRb, weapon.grip);
                lJoint = addConfigurableJoint(lHandRb, weapon.grip);
                break;
            }
        }

        weapon.grip.isKinematic = false;
    }

    private ConfigurableJoint addConfigurableJoint(Rigidbody from, Rigidbody to)
    {
        ConfigurableJoint joint = from.gameObject.AddComponent<ConfigurableJoint>();
        joint.connectedBody = to;
        joint.xMotion = ConfigurableJointMotion.Locked;
        joint.yMotion = ConfigurableJointMotion.Locked;
        joint.zMotion = ConfigurableJointMotion.Locked;
        joint.angularXMotion = ConfigurableJointMotion.Locked;
        joint.angularYMotion = ConfigurableJointMotion.Locked;
        joint.angularZMotion = ConfigurableJointMotion.Locked;
        joint.rotationDriveMode = RotationDriveMode.Slerp;
        joint.projectionMode = JointProjectionMode.PositionAndRotation;
        joint.projectionDistance = 0f;
        joint.massScale = .1f;
        joint.connectedMassScale = 10f;
        joint.enablePreprocessing = false;
        return joint;
    }
}