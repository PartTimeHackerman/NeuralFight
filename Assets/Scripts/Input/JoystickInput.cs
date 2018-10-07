using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

class JoystickInput : InputController
{
    public Image singleJoystickBackgroundImage;
    public bool singleJoyStickAlwaysVisible = true;
    public float scale = 1.5f;
    private Image singleJoystickHandleImage;
    private SingleJoystick singleJoystick;
    private int singleSideFingerID = 0;

    void Start()
    {
        singleJoystick = singleJoystickBackgroundImage.GetComponent<SingleJoystick>();
        singleJoystickBackgroundImage.enabled = singleJoyStickAlwaysVisible;
        singleJoystickHandleImage = singleJoystick.transform.GetChild(0).GetComponent<Image>();
        singleJoystickHandleImage.enabled = singleJoyStickAlwaysVisible;
        singleJoystick.transform.localScale = new Vector3(scale, scale, scale);

        /*if (Input.touchCount > 0 || Input.anyKey)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 p = Input.mousePosition;
                var currentPosition = singleJoystickBackgroundImage.rectTransform.position;
                currentPosition.x = p.x + singleJoystickBackgroundImage.rectTransform.sizeDelta.x * scale / 2;
                currentPosition.y = p.y - singleJoystickBackgroundImage.rectTransform.sizeDelta.y * scale / 2;
                if (p.x < Screen.width / 2f)
                    singleJoystickBackgroundImage.rectTransform.position = currentPosition;
            }}*/
    }

    public override void Began(Touch touch)
    {
        Vector3 p = touch.position;
        var currentPosition = singleJoystickBackgroundImage.rectTransform.position;
        currentPosition.x = p.x + singleJoystickBackgroundImage.rectTransform.sizeDelta.x * scale / 2;
        currentPosition.y = p.y - singleJoystickBackgroundImage.rectTransform.sizeDelta.y * scale / 2;
        singleJoystickBackgroundImage.rectTransform.position = currentPosition; 

        singleJoystickBackgroundImage.enabled = true;
        singleJoystickBackgroundImage.rectTransform.GetChild(0).GetComponent<Image>().enabled = true;
    }

    public override void Moved(Touch touch)
    {

    }

    public override void Ended(Touch touch)
    {
        singleJoystickBackgroundImage.enabled = singleJoyStickAlwaysVisible;
        singleJoystickHandleImage.enabled = singleJoyStickAlwaysVisible;
        RectTransform rt = singleJoystick.GetComponent(typeof(RectTransform)) as RectTransform;

        singleJoystickBackgroundImage.rectTransform.position =
            new Vector3(rt.sizeDelta.x * 1.6f, rt.sizeDelta.y * .1f, 0);
    }
}
