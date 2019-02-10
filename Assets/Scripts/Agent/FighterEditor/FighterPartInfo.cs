using DG.Tweening;
using TMPro;
using UnityEngine;

public class FighterPartInfo : MonoBehaviour
{
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Level;
    public TextMeshProUGUI HP;
    public TextMeshProUGUI SP;
    public TextMeshProUGUI SPReg;
    public TextMeshProUGUI Type;
    public TextMeshProUGUI Mass;
    public TextMeshProUGUI UpgradeText;

    public ScrollListItem itemToInfo;
    private bool Visible = false;
    public WeaponInfo WeaponInfo;
    private FighterPart CurrentPart;

    public ScrollListItem ItemToInfo
    {
        get { return itemToInfo; }
        set
        {
            itemToInfo = value;
            //itemToInfo.OnChooseItem += (item, choose) => { SetWeapon((Weapon) item.Item); };
        }
    }

    private void Awake()
    {
        transform.localPosition = ObjectsPositions.FighterPartInfoHidden;
    }

    public void SetPart(ScrollListItem item)
    {
        if (item.Destroyed)
        {
            return;
        }

        if (!Visible)
        {
            WeaponInfo.Hide();
            Show();
        }

        itemToInfo = item;
        FighterPart fighterPart = (FighterPart) item.Item;
        CurrentPart = fighterPart;
        Name.text = fighterPart.Name;
        Level.text = fighterPart.Level.ToString();
        HP.text = ThousandsConverter.ToKs(fighterPart.MaxHP);
        SP.text = ThousandsConverter.ToKs(fighterPart.MaxSP);
        SPReg.text = ThousandsConverter.ToKs(fighterPart.SpRegen);
        Type.text = fighterPart.ItemMaterial.ToString();
        Mass.text = fighterPart.Mass.ToString("F1") + "kg";
    }

    public void Show()
    {
        transform.DOLocalMove(ObjectsPositions.FighterPartInfoVisible, .3f).SetEase(Ease.OutQuart);
        Visible = true;
    }

    public void Hide()
    {
        transform.DOLocalMove(ObjectsPositions.FighterPartInfoHidden, .3f).SetEase(Ease.OutQuart);
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
            ItemUpgrader.Get().Upgrade(i, CurrentPart);
            itemToInfo.SetItem(itemToInfo.Item);
        }

        UpgradeText.gameObject.SetActive(false);
        SetPart(itemToInfo);
    }
}