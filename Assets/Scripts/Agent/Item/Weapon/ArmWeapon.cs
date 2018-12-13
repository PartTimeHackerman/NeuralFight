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
            equipWeapon(value, weapon);
            OnChangeWeapon?.Invoke(value, weapon);
            weapon = value;
        }
    }

    public event ChangeWeapon OnChangeWeapon;

    public delegate void ChangeWeapon(Weapon newWeapon, Weapon oldWeapon);

    public HandAction HandAction;

    public Transform Hand;
    public Rigidbody Arm;

    private ConfigurableJoint Joint;
    private bool unEquip = false;

    private Rigidbody equippingRb;


    protected virtual void Awake()
    {
        //OnChangeWeapon += equipWeapon;
        OnChangeWeapon += HandAction.equipAction;
    }

    private void FixedUpdate()
    {
        if (unEquip)
        {
            unEquipWeapon(weapon);
        }
    }


    public void ReEquipWeapon()
    {
        equipWeapon(weapon, weapon);
    }

    public void equipWeapon(Weapon newWeapon, Weapon oldWeapon)
    {
        unEquipWeapon(oldWeapon);
        if (newWeapon == null)
        {
            return;
        }

        weapon = newWeapon;
        //weapon.transform.parent = gameObject.transform;
        weapon.Equipped = true;
        Action first = () => { setWeaponPos(weapon); };
        Action second = () => { SetWeaponJoints(weapon); };
        StartCoroutine(Waiter.WaitForFrames(1, first, second));
    }

    public void unEquipWeapon(Weapon oldWeapon)
    {
        if (oldWeapon != null)
        {
            oldWeapon.Equipped = false;
            oldWeapon.transform.parent = null;
            if (Joint != null) Destroy(Joint);
            weapon = null;
        }
    }

    private void setWeaponPos(Weapon weapon)
    {
        Vector3 weaponPos = new Vector3();
        Quaternion weaponRot = new Quaternion();
        weaponPos = Hand.transform.position;
        weaponRot = Hand.rotation;
        equippingRb = Arm;
        weapon.transform.parent = Hand.transform;

        foreach (var child in weapon.GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = Hand.gameObject.layer;
        }

        //equippingRb.isKinematic = true;
        weapon.Rigidbody.isKinematic = true;
        //weapon.Rigidbody.MovePosition(weaponPos);
        weapon.transform.position = weaponPos;
        Vector3 rot = weaponRot.eulerAngles;
        if (weapon.WeaponDirection == WeaponDirection.UP)
        {
            rot.z -= 90f;
        }

        if (weapon.WeaponDirection == WeaponDirection.FORWARD)
        {
            rot.z -= 180f;
        }

        rot.z += weapon.BaseRotZ;
        weapon.transform.rotation = Quaternion.Euler(rot);
        
        weapon.gameObject.SetActive(false);
        weapon.gameObject.SetActive(true);
    }


    private void SetWeaponJoints(Weapon weapon)
    {
        Joint = addConfigurableJoint(Arm, weapon.Rigidbody);
        weapon.Rigidbody.isKinematic = false;
        //equippingRb.isKinematic = false;
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

        //joint.massScale = .1f;r
        //joint.connectedMassScale = 10f;
        //joint.enablePreprocessing = false;

        JointDrive slerp = joint.slerpDrive;
        slerp.positionSpring = 10000f;
        slerp.positionDamper = 10000f;
        joint.slerpDrive = slerp;
        joint.projectionAngle = 0f;
        joint.enablePreprocessing = false;
        return joint;
    }
}