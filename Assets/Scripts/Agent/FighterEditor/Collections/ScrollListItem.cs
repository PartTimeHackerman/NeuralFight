using UnityEngine;

public class ScrollListItem : MonoBehaviour
{
    public RectTransform Center;
    private Vector3 screenPoint;
    private Vector3 offset;
    private Vector3 basePos;
    private Vector3 startPos;
    private Vector3 startDragPos;
    private bool dragable = false;
    public Item Item;

    public event ChooseItem OnChooseItem;

    public delegate void ChooseItem(ScrollListItem item, bool choosed);

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
        


        if (!dragable && curPosition.x - startPos.x > .5f)
        {
            basePos = transform.position;
            dragable = true;
            OnChooseItem?.Invoke(this, false);
        }


        if (!dragable)
        {
            Vector3 posChoose = transform.position;
            //transform.position = Vector3.Lerp(posChoose, curPosition, (startPos.y - curPosition.y) / .2f);
        }

        if (dragable)
        {
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
    }
}