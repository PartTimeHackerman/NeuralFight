using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class Gap : MonoBehaviour
{
    public BoxCollider front;
    public BoxCollider back;

    public float width = 0f;
    public float totalWidth = 0f;

    public bool set = false;
    

    public void FixedUpdate()
    {
        if (set)
        {
            setGap();
            set = false;
        }
    }

    private void setGap()
    {
        totalWidth = back.transform.localPosition.x;
        Vector3 backPos = back.transform.localPosition;
        backPos.x = width + 2f;
        back.transform.localPosition = backPos;
    }
}
