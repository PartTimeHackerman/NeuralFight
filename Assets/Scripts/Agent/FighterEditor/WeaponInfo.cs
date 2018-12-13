using UnityEngine;
using UnityEngine.UI;

public class WeaponInfo : MonoBehaviour
{
    public Text Name;
    public Text Damage;
    public Text ReqSp;
    
    public ScrollListItem itemToInfo;

    public ScrollListItem ItemToInfo
    {
        get { return itemToInfo; }
        set
        {
            itemToInfo = value;
            itemToInfo.OnChooseItem += (item, choose) =>
            {
                SetWeapon((Weapon)item.Item);
            };
        }
    }

    public void SetWeapon(Weapon weapon)
    {
        Name.text = weapon.Name;
        Damage.text = ThousandsConverter.ToKs(weapon.Damage);
        ReqSp.text = ThousandsConverter.ToKs(weapon.SPReq);
    }
}