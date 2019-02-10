using UnityEngine;

public class Positioner : MonoBehaviour
{
    public bool active;

    public bool Active
    {
        get { return active; }
        set { active = value; }
    }

    private Vector3 screenPoint;
    private Vector3 offset;
    public Transform indicator;
    
    void OnMouseDown()
    {
        active = true;
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
 
    }
 
    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
 
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;
 
    }
    
    void OnMouseUp()
    {
        active = false;
        PlayerCollections.Get().SaveFighters();
    }

}