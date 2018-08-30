using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class Velocity : MonoBehaviour, ILateFixedUpdate
{
    public Vector3 velocity;
    public Vector3 angularVelocity;

    private Vector3 previousPosition;
    private Quaternion lastRotation;
    

    void Start()
    {
    }

    void FixedUpdate()
    {
        previousPosition = transform.position;
        lastRotation = transform.rotation;
        
    }

    public void LateFixedUpdate()
    {
        velocity = (transform.position - previousPosition) / Time.fixedDeltaTime;

        Quaternion deltaRotation = transform.rotation * Quaternion.Inverse(lastRotation);
        float angle = 0.0f;
        Vector3 axis = Vector3.zero;
        deltaRotation.ToAngleAxis(out angle, out axis);
        angle *= Mathf.Deg2Rad;
        if (angle > 1 || angle < -1)
        {
            Vector3 eulerRotation = new Vector3(
                Mathf.DeltaAngle(0, Mathf.Round(deltaRotation.eulerAngles.x)),
                Mathf.DeltaAngle(0, Mathf.Round(deltaRotation.eulerAngles.y)),
                Mathf.DeltaAngle(0, Mathf.Round(deltaRotation.eulerAngles.z)));
            angularVelocity = eulerRotation / Time.fixedDeltaTime * Mathf.Deg2Rad;
        }
        else 
            angularVelocity = axis * angle / Time.fixedDeltaTime;


    }
}
