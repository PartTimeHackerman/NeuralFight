using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class VerticalEffectorLearning : VerticalEffector
{
    public float currVelocity = 0f;
    private IAgent agent;
    public int episodesBoundary = 1000;
    
    void Start()
    {
        agent = GetComponent<IAgent>();
    }

    public override void addForce()
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
        currVelocity = Mathf.Lerp(velocity, 0, agent.getEpisodes() / (float) episodesBoundary);
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