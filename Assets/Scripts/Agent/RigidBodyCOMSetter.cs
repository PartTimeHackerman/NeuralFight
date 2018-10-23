using UnityEngine;

[RequireComponent(typeof(BodyParts))]
public class RigidBodyCOMSetter : MonoBehaviour
{
    private BodyParts BodyParts;

    private void Start()
    {
        BodyParts = GetComponent<BodyParts>();
        foreach (Rigidbody rigid in BodyParts.getRigids())
        {
            if (rigid.GetComponent<BoxCollider>())
            {
                rigid.centerOfMass = rigid.GetComponent<BoxCollider>().center;
            }
        }
    }
}