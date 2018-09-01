using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class VerticalEffector : MonoBehaviour
{
    public Rigidbody rigidbody;
    public Rigidbody referenceRb;
    public float velocity;
    public bool enable = false;
    private Vector3 dir;
    private Vector3 cross;
    private Vector3 side;
    private float up;

    void FixedUpdate()
    {
        if (enable)
        {
            addForce();
        }
    }

    public void addForce()
    {
        dir = referenceRb.transform.position - rigidbody.transform.position;
        dir.Normalize();
        side = Vector3.Cross(dir, -rigidbody.transform.right);
        cross = Vector3.Cross(dir, side).normalized;
        if (rigidbody.transform.position.x > referenceRb.transform.position.x)
            cross.x *= -1;
        cross.y = Mathf.Abs(cross.y);
        up = Mathf.Abs(((rigidbody.transform.up.y + 1) / 2) - 1);
        cross.x *= up;
        cross.y *= up;
        rigidbody.AddForce(cross.x * velocity, cross.y * velocity, 0);
        referenceRb.AddForce(-cross.x * velocity, -cross.y * velocity, 0);
        
    }

}
