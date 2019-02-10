using UnityEngine;

public class WeaponDistanceEquipper : MonoBehaviour
{
    public PlayerWeaponsCollection PlayerWeaponsCollection;
    public ArmWeapon RightArmWeapon;
    public ArmWeapon LeftArmWeapon;
    private ArmWeapon armToEquip;
    public float minDist = 1f;
    public FighterSaverLoader FighterSaverLoader;
    private Fighter Fighter;

    public ScrollListItem itemToEquip;

    public ScrollListItem ItemToEquip
    {
        get { return itemToEquip; }
        set
        {
            itemToEquip = value;
            Weapon weapon = (Weapon) ItemToEquip.Item;
            itemToEquip.OnChooseItem += (item, choose) =>
            {
                if (armToEquip != null)
                {
                    Weapon oldWeapon = armToEquip.Weapon;
                    oldWeapon.EnableOutlines(false);
                    armToEquip.Weapon = weapon;
                    //itemToEquip.gameObject.SetActive(false);
                    Destroy(itemToEquip.gameObject);
                    
                    Storage.SaveWeapon(oldWeapon);
                    Storage.SaveWeapon(weapon);
                    Storage.SaveFighter(Fighter);
                    
                    PlayerWeaponsCollection.AddToScrollList(oldWeapon);
                    //FighterSaverLoader.SaveFighter(Fighter);
                    itemToEquip = null;
                    armToEquip = null;
                }
            };
        }
    }

    public void SetFighter(Fighter fighter)
    {
        Fighter = fighter;
        RightArmWeapon = fighter.RightArmWeapon;
        LeftArmWeapon = fighter.LeftArmWeapon;
    }

    private void FixedUpdate()
    {
        if (ItemToEquip != null)
        {
            Vector2 itemPos = ItemToEquip.transform.position;
            Vector2 rightHandPos = RightArmWeapon.Hand.position;
            Vector2 leftHandPos = LeftArmWeapon.Hand.position;
            float distToRight = Vector2.Distance(itemPos, rightHandPos);
            float distToLeft = Vector2.Distance(itemPos, leftHandPos);

            if (distToRight < minDist || distToLeft < minDist)
            {
                if (armToEquip != null)
                {
                    
                    armToEquip.Weapon.EnableOutlines(false);
                }

                armToEquip = distToRight < distToLeft ? RightArmWeapon : LeftArmWeapon;
                armToEquip.Weapon.EnableOutlines(true);

            }
            else
            {
                if (armToEquip != null)
                {
                    armToEquip.Weapon.EnableOutlines(false);
                }
                armToEquip = null;
            }

        }
    }
}