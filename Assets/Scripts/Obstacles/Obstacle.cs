using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

abstract class Obstacle : MonoBehaviour
{
    public float totalWidth = 0f;

    public abstract void setRandom();

    public void setXPosition(float xPos)
    {
        Vector3 objPos = transform.position;
        objPos.x = xPos;
        transform.position = objPos;
    }

    public ColorShifterManager GetColorShifterManager()
    {
        return GetComponent<ColorShifterManager>();
    }
}
