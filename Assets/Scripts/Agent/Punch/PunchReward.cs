using Assets.Scripts.Agent;
using UnityEngine;

public class PunchReward : MonoBehaviour
{
    public Rigidbody end;
    public float reward = 0f;

    void Start()
    {
        //InvokeRepeating("getReward", 0f, .1f);
    }

    public float getReward(Vector3 pointPos)
    {
        Vector3 relPos = pointPos - end.position;
        relPos = relPos.normalized;
        Vector3 endVel = end.velocity;

        //Vector2 mul = new Vector2(relPos.x * endVel.x, relPos.y * endVel.y);
        float dirVel = relPos.x * endVel.x + relPos.y * endVel.y;
        reward = dirVel;
        return reward;
    }
}