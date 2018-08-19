using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


class VelocityReward : MonoBehaviour
{

    public Vector2 direction;
    public float maxVel;
    public Rigidbody rigidbody;
    public float reward = 0f;

    public VelocityReward(Vector2 direction, float maxVel, Rigidbody rigidbody)
    {
        this.direction = direction;
        this.maxVel = maxVel;
        this.rigidbody = rigidbody;
    }

    public float getReward()
    {
        Vector3 vel = rigidbody.velocity;
        float dirVel = direction.x * vel.x + direction.y * vel.y;
        dirVel = Mathf.Clamp(dirVel, -maxVel, maxVel);
        reward = dirVel / maxVel;
        return reward;
    }
}
