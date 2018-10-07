using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


class DoubleInput : MonoBehaviour
{
    
    private int leftSideFingerID = 0;
    private int rightSideFingerID = 0;

    public InputController leftController;
    public InputController rightController;

    void FixedUpdate()
    {
        if (Input.touchCount > 0)
        {
            Touch[] myTouches = Input.touches;
            
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (myTouches[i].phase == TouchPhase.Began)
                {
                    if (myTouches[i].position.x < Screen.width / 2)
                    {
                        leftSideFingerID = myTouches[i].fingerId;
                        leftController.Began(myTouches[i]);
                    }
                    
                    if (myTouches[i].position.x > Screen.width / 2)
                    {
                        rightSideFingerID = myTouches[i].fingerId;

                        rightController.Began(myTouches[i]);
                    }
                }
                if (myTouches[i].phase == TouchPhase.Moved)
                {
                    if (myTouches[i].fingerId == leftSideFingerID)
                    {
                        leftController.Moved(myTouches[i]);
                    }

                    if (myTouches[i].fingerId == rightSideFingerID)
                    {
                        rightController.Began(myTouches[i]);
                    }
                }
                if (myTouches[i].phase == TouchPhase.Ended)
                {
                    if (myTouches[i].fingerId == leftSideFingerID)
                    {
                        leftController.Ended(myTouches[i]);
                    }
                    
                    if (myTouches[i].fingerId == rightSideFingerID)
                    {
                        rightController.Ended(myTouches[i]);
                    }
                }
            }
        }
    }
}