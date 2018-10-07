using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

abstract class Obstacle : Poolable
{
    public float totalWidth = 0f;
    public ObstacleType type;
    

    public ColorShifterManager colorShifterManager;

    public virtual void setRandom()
    {
        GetComponent<SpriteShadows>().resetShadows();
    }

    public virtual void setXPosition(float xPos)
    {
        Vector3 objPos = transform.position;
        objPos.x = xPos;
        transform.position = objPos;
        colorShifterManager = GetComponent<ColorShifterManager>();
    }
    
}

public enum ObstacleType
{
    RAMP,
    STEP,
    GAP,
    BOOST,
    CANNON
}
