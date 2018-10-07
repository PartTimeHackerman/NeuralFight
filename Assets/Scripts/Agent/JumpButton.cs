using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

class JumpButton : MonoBehaviour
{
    public ModelInput modelInput;
    public string Name;

    void Start()
    {
        EventTrigger trigger = GetComponent<EventTrigger>();

        EventTrigger.Entry entryDown = new EventTrigger.Entry();
        entryDown.eventID = EventTriggerType.PointerDown;
        entryDown.callback.AddListener((data) => { SetDownState(); });
        trigger.triggers.Add(entryDown);

        EventTrigger.Entry entryUp = new EventTrigger.Entry();
        entryUp.eventID = EventTriggerType.PointerUp;
        entryUp.callback.AddListener((data) => { SetUpState(); });
        trigger.triggers.Add(entryUp);
    }

    public void SetDownState()
    {
        modelInput.JumpSetDownState();
    }

    public void SetUpState()
    {
        modelInput.jumping = false;
    }
}
