using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Collections;

public class AntReward : MonoBehaviour
{
    public bool debug = false;
    private float baseDistanceFeetsTorso;
    private BodyParts bodyParts;
    private Dictionary<string, GameObject> namedParts;
    private PhysicsUtils physics;

    private Rigidbody root;
    public float reward;
    public int step = 0;
    public int maxStep = 300;
    public float up;


    private void Start()
    {
        bodyParts = GetComponent<BodyParts>();
        namedParts = bodyParts.getNamedParts();
        root = bodyParts.root;

        if (debug)
            InvokeRepeating("log", 0.0f, .1f);
    }

    private float getXVelReward()
    {
        return Mathf.Clamp(root.velocity.x / 10f, -1f, 1f);
    }

    public float getReward()
    {
        reward = getXVelReward();
        return reward;
    }

   

    public bool terminated(int step)
    {
        this.step = step;
        bool terminated = step >= maxStep || root.transform.up.y < 0 ;
        return terminated;
    }


    public void log()
    {
        getReward();
        up = root.transform.up.y;
    }
}