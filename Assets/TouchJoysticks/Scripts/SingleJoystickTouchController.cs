﻿/*
about this script: 

if this single joystick is not set to stay in a fixed position
 this script makes it appear and disappear where the screen was touched (if set to always stay visible this single joystick will just disappear from its current position and appear where the screen was touches and stay visible at the new position)

if this joystick is set to stay in a fixed position
 this script makes it appear and disappear if the user touches within the area of its background image (even if it is not currently visible)
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SingleJoystickTouchController : MonoBehaviour
{
    public Image singleJoystickBackgroundImage; // background image of the single joystick (the joystick's handle (knob) is a child of this image and moves along with it)
    public bool singleJoyStickAlwaysVisible = true; // value from single joystick that determines if the single joystick should be always visible or not

    public float scale = 1.5f;
    private Image singleJoystickHandleImage; // handle (knob) image of the single joystick
    private SingleJoystick singleJoystick; // script component attached to the single joystick's background image
    private int singleSideFingerID = 0; // unique finger id for touches on the left-side half of the screen

    void Start()
    {
        if (singleJoystickBackgroundImage.GetComponent<SingleJoystick>() == null)
        {
            Debug.LogError("There is no joystick attached to this script.");
        }
        else
        {
            singleJoystick = singleJoystickBackgroundImage.GetComponent<SingleJoystick>(); // gets the single joystick script
            singleJoystickBackgroundImage.enabled = singleJoyStickAlwaysVisible; // sets single joystick background image to be always visible or not
        }

        if (singleJoystick.transform.GetChild(0).GetComponent<Image>() == null)
        {
            Debug.LogError("There is no joystick handle (knob) attached to this script.");
        }
        else
        {
            singleJoystickHandleImage = singleJoystick.transform.GetChild(0).GetComponent<Image>(); // gets the handle (knob) image of the single joystick
            singleJoystickHandleImage.enabled = singleJoyStickAlwaysVisible; // sets single joystick handle (knob) image to be always visible or not
        }

        singleJoystick.transform.localScale = new Vector3(scale, scale, scale);
    }

    void FixedUpdate()
    {
        // if the screen has been touched
        /*if (Input.anyKey)
        {
            singleJoystickBackgroundImage.enabled = true;
            singleJoystickHandleImage.enabled = true;
        }
        else
        {
            singleJoystickBackgroundImage.enabled = false;
            singleJoystickHandleImage.enabled = false;
        }*/
        if (Input.touchCount > 0 || Input.anyKey)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 p = Input.mousePosition;
                var currentPosition = singleJoystickBackgroundImage.rectTransform.position; // gets the current position of the single joystick
                currentPosition.x = p.x + singleJoystickBackgroundImage.rectTransform.sizeDelta.x * scale / 2; // calculates the x position of the single joystick to where the screen was touched
                currentPosition.y = p.y - singleJoystickBackgroundImage.rectTransform.sizeDelta.y * scale / 2; // calculates the y position of the single joystick to where the screen was touched
                //currentPosition.z = 1f;
                //currentPosition = Camera.main.ScreenToWorldPoint(currentPosition);
                //currentPosition.z = 10f;
                // keeps this single joystick within the screen
                //currentPosition.x = Mathf.Clamp(currentPosition.x, 0 + singleJoystickBackgroundImage.rectTransform.sizeDelta.x, Screen.width);
                //currentPosition.y = Mathf.Clamp(currentPosition.y, 0, Screen.height - singleJoystickBackgroundImage.rectTransform.sizeDelta.y);
                if (p.x < Screen.width / 2f)
                    singleJoystickBackgroundImage.rectTransform.position = currentPosition; // sets the position of the single joystick to where the screen was touched (limited to the left half of the screen)


            }



            Touch[] myTouches = Input.touches; // gets all the touches and stores them in an array

            // loops through all the current touches
            for (int i = 0; i < Input.touchCount; i++)
            {
                // if this touch just started (finger is down for the first time), for this particular touch 
                if (myTouches[i].phase == TouchPhase.Began)
                {
                    singleSideFingerID = myTouches[i].fingerId; // stores the unique id for this touch that happened on the left-side half of the screen

                    // if the single joystick will drag with any touch (single joystick is not set to stay in a fixed position)
                    if (singleJoystick.joystickStaysInFixedPosition == false)
                    {
                        var currentPosition = singleJoystickBackgroundImage.rectTransform.position; // gets the current position of the single joystick
                        currentPosition.z = 0f;
                        currentPosition.x = myTouches[i].position.x + singleJoystickBackgroundImage.rectTransform.sizeDelta.x; // calculates the x position of the single joystick to where the screen was touched
                        currentPosition.y = myTouches[i].position.y - singleJoystickBackgroundImage.rectTransform.sizeDelta.y; // calculates the y position of the single joystick to where the screen was touched
                        //currentPosition = Camera.main.ScreenToWorldPoint(currentPosition);
                        //currentPosition.z = 100f;
                        // keeps this single joystick within the screen
                        //currentPosition.x = Mathf.Clamp(currentPosition.x, 0 + singleJoystickBackgroundImage.rectTransform.sizeDelta.x, Screen.width);
                        //currentPosition.x = Mathf.Clamp(currentPosition.x, 0, Screen.width);
                        //currentPosition.y = Mathf.Clamp(currentPosition.y, 0, Screen.height - singleJoystickBackgroundImage.rectTransform.sizeDelta.y);

                        if (myTouches[i].position.x < Screen.width / 2f)
                            singleJoystickBackgroundImage.rectTransform.position = currentPosition; // sets the position of the single joystick to where the screen was touched (limited to the left half of the screen)

                        // enables single joystick on touch
                        singleJoystickBackgroundImage.enabled = true;
                        singleJoystickBackgroundImage.rectTransform.GetChild(0).GetComponent<Image>().enabled = true;
                    }
                    else
                    {

                        // single joystick stays fixed, does not set position of single joystick on touch

                        // if the touch happens within the fixed area of the single joystick's background image within the x coordinate
                        if ((myTouches[i].position.x <= singleJoystickBackgroundImage.rectTransform.position.x) && (myTouches[i].position.x >= (singleJoystickBackgroundImage.rectTransform.position.x - singleJoystickBackgroundImage.rectTransform.sizeDelta.x)))
                        {
                            // and the touch also happens within the single joystick's background image y coordinate
                            if ((myTouches[i].position.y >= singleJoystickBackgroundImage.rectTransform.position.y) && (myTouches[i].position.y <= (singleJoystickBackgroundImage.rectTransform.position.y + singleJoystickBackgroundImage.rectTransform.sizeDelta.y)))
                            {
                                // makes the single joystick appear 
                                singleJoystickBackgroundImage.enabled = true;
                                singleJoystickBackgroundImage.rectTransform.GetChild(0).GetComponent<Image>().enabled = true;
                            }
                        }
                    }
                }

                

                if (myTouches[i].phase == TouchPhase.Ended)
                {
                    // if this touch is the touch that began on the left half of the screen
                    if (myTouches[i].fingerId == singleSideFingerID)
                    {
                        // makes the single joystick disappear or stay visible
                        singleJoystickBackgroundImage.enabled = singleJoyStickAlwaysVisible;
                        singleJoystickHandleImage.enabled = singleJoyStickAlwaysVisible;
                    }
                }
            }
        }
        else
        {
            RectTransform rt = singleJoystick.GetComponent(typeof(RectTransform)) as RectTransform;
            //singleJoystickBackgroundImage.rectTransform.position = Camera.main.ScreenToWorldPoint(new Vector3(rt.sizeDelta.x * 1.6f, rt.sizeDelta.y * .1f, 0));
            singleJoystickBackgroundImage.rectTransform.position = new Vector3(rt.sizeDelta.x * 1.6f, rt.sizeDelta.y * .1f, 0);
        }
    }
}
