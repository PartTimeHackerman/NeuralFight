using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class TerminateDuel : MonoBehaviour
{
    public bool debug = false;
    public float arenaLen = 4;
    public float fallHeight = .3f;
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
        Vector3 headPos = bodyParts.getNamedRigids()["head_end"].transform.position;
        return COM.y < fallHeight || headPos.y < fallHeight;
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
