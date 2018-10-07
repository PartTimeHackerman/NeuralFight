using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

class SwipeArea : MonoBehaviour
{
    public Vector2 down;
    public Vector2 up;
    public Vector2 dir;
    public float distance;
    public ParticleSystem swipeParticles;
    public ModelInput ModelInput;
    public float force = 100f;

    void Start()
    {
        EventTrigger trigger = GetComponent<EventTrigger>();

        EventTrigger.Entry entryDown = new EventTrigger.Entry();
        entryDown.eventID = EventTriggerType.InitializePotentialDrag;
        entryDown.callback.AddListener(OnBeginDrag);
        trigger.triggers.Add(entryDown);

        EventTrigger.Entry entryUp = new EventTrigger.Entry();
        entryUp.eventID = EventTriggerType.PointerUp;
        entryUp.callback.AddListener(OnEndDrag);
        trigger.triggers.Add(entryUp);

        EventTrigger.Entry entryDrag = new EventTrigger.Entry();
        entryDrag.eventID = EventTriggerType.Drag;
        entryDrag.callback.AddListener(OnDrag);
        trigger.triggers.Add(entryDrag);
    }

    public void OnBeginDrag(BaseEventData data)
    {
        Vector3 pos = data.currentInputModule.input.mousePosition;
        down = pos;

        Vector3 parPos = Camera.main.ScreenToWorldPoint(pos);
        parPos.z = swipeParticles.transform.position.z;
        swipeParticles.transform.position = parPos;
        swipeParticles.Play();
    }

    public void OnDrag(BaseEventData data)
    {
        
        Vector3 pos = data.currentInputModule.input.mousePosition;
        pos = Camera.main.ScreenToWorldPoint(pos);
        pos.z = swipeParticles.transform.position.z;
        swipeParticles.transform.position = pos;

    }

    public void OnEndDrag(BaseEventData data)
    {
        Vector3 pos = data.currentInputModule.input.mousePosition;
        pos.z = 1f;
        up = pos;
        Vector2 screen = new Vector2(Screen.width, Screen.width);
        distance = Vector2.Distance(down / screen, up/screen);
        
        swipeParticles.Stop();
        dir = up - down;
        dir = dir.normalized;
        float swipeForce = distance * force;
        swipeForce = Mathf.Min(swipeForce, 40f);
        ModelInput.addVel(dir, swipeForce, ForceMode.VelocityChange);
    }
}
