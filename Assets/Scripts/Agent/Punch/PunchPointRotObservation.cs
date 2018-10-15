using UnityEngine;

public class PunchPointRotObservation : MonoBehaviour
{
    public Transform target;
    public float rot = 0f;
    
    void Start()
    {
        //InvokeRepeating("setRelRot", 0f, .1f);
    }

    void setRelRot()
    {
        Vector3 relativePos = target.position - transform.position;
        //Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        //Vector3 rotEul = rotation.eulerAngles;
        float rotAng =  Vector3.Angle(relativePos, transform.right);
        float rotClamped = 0f;
        if (rotAng <= 180f)
            rotClamped = rotAng / 180f;
        else
            rotClamped = ((rotAng - 180f) / 180f) - 1f;

        if (relativePos.y > 0 )
        {
            rotClamped *= -1f;
        }
        rot = rotClamped;
    }
}