using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpToLimit : MonoBehaviour
{

    public float upVel = 10f;
    public float limit = 1.6f;
    public float vel;
    private Rigidbody rb;
    public bool pos = true;

    void Start()
    {
        pos = true;
        upVel = 10f;
        limit = 1.6f;
        rb = GetComponent<Rigidbody>();
        //limit = rb.position.y;
    }

    public UpToLimit(Rigidbody rigidbody, float upVel, float limit)
    {
        pos = false;
        this.upVel = upVel;
        this.limit = limit;
        this.rb = rigidbody;
    }

    void FixedUpdate()
    {
        if (rb.position.y < limit && rb.velocity.y < 0 && !pos)
        {

            vel = upVel * Mathf.Abs((rb.position.y / limit) - 1);
            rb.AddForce(0f, vel, 0f);
        }
        else
        {
            Vector3 currPos = rb.transform.position;
            if (currPos.y > limit)
            {
                float dist = 1.9f - currPos.y;
                rb.AddForce(0f, dist * upVel, 0f);
            }


        }

    }

    public void addVel()
    {
        if (rb.position.y < limit)
        {

            vel = upVel * Mathf.Abs((rb.position.y / limit) - 1);
            rb.AddForce(0f, vel, 0f);
        }
    }
}
