using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class TerminateDuel : MonoBehaviour
{
    public bool debug = false;
    public int arenaLen = 4;
    public float fallHeight = 1.3f;
    public bool pastArena = false;
    public bool fallen = false;
    public bool terminated = false;

    private BodyParts bodyParts;
    private PhysicsUtils physicsUtils;

    private Vector3 COM;
    



    private void Start()
    {
        bodyParts = GetComponent<BodyParts>();
        physicsUtils = PhysicsUtils.get();


        if (debug)
            InvokeRepeating("isTerminated", 0.0f, .1f);
    }

    private bool isPastArena()
    {
        return Mathf.Abs(COM.x) > arenaLen;
    }

    private bool isFallen()
    {
        return COM.y < fallHeight;
    }

    public bool isTerminated()
    {
        COM = physicsUtils.getCenterOfMass(bodyParts.getRigids());
        pastArena = isPastArena();
        fallen = isFallen();
        terminated = pastArena || fallen;
        return terminated;
    }





    
}
