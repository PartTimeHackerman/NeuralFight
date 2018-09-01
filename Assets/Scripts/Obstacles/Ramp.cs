using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class Ramp : MonoBehaviour
{
    public CapsuleCollider horizontal;
    public CapsuleCollider vertical;

    public float width = 0f, height = 0f;
    public bool setRamp = false;

    void Start()
    {
        setRampWH();
    }

    public void FixedUpdate()
    {
        if (setRamp)
        {
            setRampWH();
            setRamp = false;
        }
    }

    public void setRampWH()
    {
        setWidth();
        setHeight();
        setHorizontal();
    }

    public void setWidth()
    {
        Vector3 verLocPos = vertical.transform.localPosition;
        verLocPos.x = width;
        vertical.transform.localPosition = verLocPos;

    }

    public void setHeight()
    {
        vertical.height = height + .1f;
        Vector3 verCenter = vertical.center;
        verCenter.y = (height + .1f) / 2f;
        vertical.center = verCenter;
    }

    public void setHorizontal()
    {

        Vector3 widthHeight = new Vector3(width, height, 0f);
        float horizontalHeight = Vector3.Distance(Vector3.zero, widthHeight);
        horizontal.height = horizontalHeight + .1f;
        Vector3 horCenter = horizontal.center;
        horCenter.x = (horizontalHeight + .1f) / 2f;
        horizontal.center = horCenter;

        float horizontalAngle = Vector2.Angle(new Vector3(width, 0f, 0f), widthHeight);
        Vector3 horAng = horizontal.transform.rotation.eulerAngles;
        horAng.z = horizontalAngle;
        horizontal.transform.rotation = Quaternion.Euler(horAng);
    }

}
