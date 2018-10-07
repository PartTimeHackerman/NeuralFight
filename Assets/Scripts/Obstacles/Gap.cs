using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

class Gap : Obstacle
{
    public BoxCollider front;
    public BoxCollider back;

    public float width = 0f;
    //public float totalWidth = 0f;

    public bool set = false;

    void OnEnable()
    {
        type = ObstacleType.GAP;
    }

    public void FixedUpdate()
    {
        if (set)
        {
            setRandom();
            set = false;
        }
    }

    public override void setRandom()
    {
        width = Random.Range(0.5f, 5f);
        setGap();
        base.setRandom();
    }

    private void setGap()
    {
        totalWidth = width + 2f;
        Vector3 backPos = back.transform.localPosition;
        backPos.x = width + 2f;
        back.transform.localPosition = backPos;
    }
}
