using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class RayToFloor : MonoBehaviour
{

    public Rigidbody rigidbody;
    public float dist;
    void FixedUpdate()
    {
        int layerMask = 1 << rigidbody.gameObject.layer;
        
        layerMask = ~layerMask;
        RaycastHit hit;
        if (Physics.Raycast(rigidbody.transform.position, Vector3.down, out hit, 1000, layerMask))
        {
            Debug.DrawRay(transform.position, Vector3.down * hit.distance, Color.yellow);
            dist = hit.distance;
            Debug.Log(hit.distance);
        }
    }
}
