using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollListItem : MonoBehaviour
{
    public RectTransform Center;
    private Vector3 screenPoint;
    private Vector3 offset;
    private Vector3 basePos;
    private Vector3 startPos;
    private Vector3 startDragPos;
    private bool dragable = false;
    private bool selectable = false;
    public Item Item;
    public Text ItemText;
    public TextMeshPro TextMesh;
    public bool Destroyed = false;
    
    public event DragItem OnDragItem;
    public delegate void DragItem(ScrollListItem item);
    
    public event SelectItem OnSelectItem;
    public delegate void SelectItem(ScrollListItem item);
    
    public event ChooseItem OnChooseItem;
    public delegate void ChooseItem(ScrollListItem item, bool choosed);


    public void SetItem(Item item)
    {
        Item = item;
        item.transform.parent = Center.transform;
        //item.transform.position = scrollListItem.transform.position - item.Center.position;
        Vector3 wepRot = item.transform.rotation.eulerAngles;
        wepRot.z = -30;
        item.transform.rotation = Quaternion.Euler(wepRot);
        item.transform.localPosition = item.transform.position - item.Center.position;
        TextMesh.text = item.Name + " lv." + item.Level;
    }

    void OnMouseDown()
    {
        Vector3 pos = gameObject.transform.position;
        startPos = pos;
        //pos.z = -1.33f;
        screenPoint = Camera.main.WorldToScreenPoint(pos);
        offset = gameObject.transform.position -
                 Camera.main.ScreenToWorldPoint(
                     new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        startDragPos = offset;
        
    }

    void OnMouseDrag()
    {
        Vector3 pos = gameObject.transform.position;
        screenPoint = Camera.main.WorldToScreenPoint(pos);
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;


        if (!dragable
            && ((Item.GetType() == typeof(Weapon) && curPosition.x - startPos.x > .5f)
                || (Item.GetType() == typeof(FighterPart) && curPosition.x - startPos.x < -0.5f)))
        {
            basePos = transform.position;
            dragable = true;
            OnChooseItem?.Invoke(this, false);
        }
        
        if (!selectable && Mathf.Abs(curPosition.y - startPos.y) < .01f)
        {
            selectable = true;
        }

        if (!dragable)
        {
            Vector3 posChoose = transform.position;
            //transform.position = Vector3.Lerp(posChoose, curPosition, (startPos.y - curPosition.y) / .2f);
        }

        if (dragable)
        {
            OnDragItem?.Invoke(this);
            Vector3 posItem = gameObject.transform.position;
            posItem.z = ObjectsPositions.FighterEditorPos.z - .33f;
            Vector3 screenPointItem = Camera.main.WorldToScreenPoint(posItem);
            Vector3 curScreenPointItem = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPointItem.z);
            Vector3 curPositionItem = Camera.main.ScreenToWorldPoint(curScreenPointItem) + offset;
            curPositionItem.z = ObjectsPositions.FighterEditorPos.z - .33f;
            transform.position = curPositionItem;
        }
    }

    void OnMouseUp()
    {
        if (dragable)
        {
            dragable = false;
            OnChooseItem?.Invoke(this, true);

            transform.position = basePos;
        }

        if (selectable)
        {
            selectable = false;
            OnSelectItem?.Invoke(this);
        }
    }
}