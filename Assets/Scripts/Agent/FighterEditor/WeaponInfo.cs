using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInfo : MonoBehaviour
{
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Level;
    public TextMeshProUGUI Damage;
    public TextMeshProUGUI ReqSp;
    public TextMeshProUGUI Type;
    public TextMeshProUGUI Mass;
    public TextMeshProUGUI UpgradeText;

    public bool Visible = false;
    public FighterPartInfo FighterPartInfo;
    
    public ScrollListItem itemToInfo;
    private Weapon CurrentWeapon;


    private void Awake()
    {
        transform.localPosition = ObjectsPositions.WeaponInfoHidden;
    }

    
    public void SetWeapon(ScrollListItem item)
    {
        if (item.Destroyed)
        {
            return;
        }
        if (!Visible)
        {
            FighterPartInfo.Hide();
            Show();
        }

        itemToInfo = item;
        Weapon weapon = (Weapon) item.Item;
        CurrentWeapon = weapon;
        Name.text = weapon.Name;
        Level.text = weapon.Level.ToString();
        Damage.text = ThousandsConverter.ToKs(weapon.Damage);
        ReqSp.text = ThousandsConverter.ToKs(weapon.SPReq);
        Type.text = weapon.ItemMaterial.ToString();
        Mass.text = weapon.Mass.ToString("F1") + "kg";
    }

    public void Show()
    {
        transform.DOLocalMove(ObjectsPositions.WeaponInfoVisible, .3f).SetEase(Ease.OutQuart);
        Visible = true;
    }
    
    public void Hide()
    {
        transform.DOLocalMove(ObjectsPositions.WeaponInfoHidden, .3f).SetEase(Ease.OutQuart);
        Visible = false;
    }

    public void ShowUpgrade(ScrollListItem item)
    {
        if (Vector2.Distance(item.transform.position, transform.position) < 1f)
        {
            UpgradeText.gameObject.SetActive(true);
        }
        else
        {
            UpgradeText.gameObject.SetActive(false);
        }
    }
    
    public void Upgrade(ScrollListItem item)
    {
        if (Vector2.Distance(item.transform.position, transform.position) < 1f)
        {
            Item i = item.Item;
            i.transform.parent = null;
            item.Destroyed = true;
            Destroy(item.gameObject);
            ItemUpgrader.Get().Upgrade(i, CurrentWeapon);
            itemToInfo.SetItem(itemToInfo.Item);
        }
        UpgradeText.gameObject.SetActive(false);
        SetWeapon(itemToInfo);
    }

}