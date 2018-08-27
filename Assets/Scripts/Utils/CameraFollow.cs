using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class CameraFollow : MonoBehaviour
{
    public GameObject obj;
    public float xOffset = 0f;
    public float yOffset = 3.6f;

    void Update()
    {
        Vector3 objPos = obj.transform.position;
        gameObject.transform.position = new Vector3(objPos.x + xOffset, objPos.y + yOffset, -10f);
    }
}
