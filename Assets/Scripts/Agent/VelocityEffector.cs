using System;
using UnityEngine;

public class VelocityEffector : MonoBehaviour
{
    public BodyParts BodyParts;
    public LocomotyionType LocomotyionType;
    public bool left = true;
    public float velocity;
    private Vector3 velDir;
    private Rigidbody torso;
    private Rigidbody butt;
    public bool enable = false;

    void Start()
    {
        torso = BodyParts.getNamedRigids()["torso"];
        butt = BodyParts.getNamedRigids()["butt"];

        switch (LocomotyionType)
        {
            case LocomotyionType.WALK_FORWARD:
                velDir = left ? Vector3.right : Vector3.left;
                break;
            case LocomotyionType.WALK_BACKWARD:
                velDir = left ? Vector3.left : Vector3.right;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    void FixedUpdate()
    {
        if (enable)
        {
            addForce();
        }
    }

    private void addForce()
    {
        torso.AddForce(velocity * velDir, ForceMode.VelocityChange);
    }
}