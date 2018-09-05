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
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => { SetDownState(); });
        trigger.triggers.Add(entry);
    }

    public void SetDownState()
    {
        modelInput.JumpSetDownState();
    }
}
