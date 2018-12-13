﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class VerticalEffector : MonoBehaviour
{
    public Rigidbody rigidbody;
    public Rigidbody referenceRb;
    public float velocity;
    public bool enable = false;
    public bool left = true;
    public bool debug = false;
    protected Vector3 dir;
    protected Vector3 cross;
    protected Vector3 side;
    protected float up;

    void FixedUpdate()
    {
        if (enable)
        {
            addForce();
        }
    }

    public virtual void addForce()
    {
        if (rigidbody.CompareTag("detached") || referenceRb.CompareTag("detached"))
        {
            return;
        }

        float velocity = this.velocity;
        dir = referenceRb.transform.position - rigidbody.transform.position;
        dir.Normalize();
        if (left)
        {
            if (rigidbody.transform.position.x < referenceRb.transform.position.x)
            {
                velocity = Mathf.Min(velocity * 3.3f, 10000f);
            }
            side = Vector3.Cross(dir, -rigidbody.transform.right);
        }
        else
        {
            if (rigidbody.transform.position.x > referenceRb.transform.position.x)
            {
                velocity = Mathf.Min(velocity * 3.3f, 10000f);
            }
            side = Vector3.Cross(dir, rigidbody.transform.right);
        }

        cross = Vector3.Cross(dir, side).normalized;
        if (rigidbody.transform.position.x > referenceRb.transform.position.x)
            cross.x *= -1;



        cross.y = Mathf.Abs(cross.y);
        up = Mathf.Abs(((rigidbody.transform.up.y + 1) / 2) - 1);
        cross.x *= up;
        cross.y *= up;
        Vector2 crossVel = new Vector2(cross.x * velocity, cross.y * velocity);
        rigidbody.AddForce(crossVel);
        referenceRb.AddForce(-crossVel);
        if (debug)
        {
            Debug.DrawRay(rigidbody.transform.position, crossVel * .01f);
            Debug.DrawRay(referenceRb.transform.position, -crossVel * .01f);
        }
    }
}