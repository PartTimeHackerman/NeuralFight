
    using System.Collections.Generic;
    using UnityEngine;

public abstract class TwoHandedAction : MonoBehaviour
{
    public bool activate = false;
    public bool done = false;
    public Transform target;
    protected BodyParts BodyParts;

    protected Rigidbody rUpperArm;
    protected Rigidbody lUpperArm;


    protected virtual void Start()
    {
        BodyParts = GetComponent<BodyParts>();
        foreach (KeyValuePair<string, Rigidbody> keyValuePair in BodyParts.getNamedRigids())
        {
            if (keyValuePair.Key.Contains("rupper")) rUpperArm = keyValuePair.Value;
            if (keyValuePair.Key.Contains("lupper")) lUpperArm = keyValuePair.Value;
        }

        BodyParts rBodyParts = rUpperArm.gameObject.AddComponent<BodyParts>();
        rBodyParts.root = BodyParts.root;
        BodyParts lBodyParts = lUpperArm.gameObject.AddComponent<BodyParts>();
        lBodyParts.root = BodyParts.root;

    }

    private void FixedUpdate()
    {
        if (activate)
        {
            done = false;
            TakeAction();
            if (done) activate = false;
        }
    }
        
    protected abstract void TakeAction();
    
}