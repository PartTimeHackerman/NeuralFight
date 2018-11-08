using UnityEngine;

public abstract class WeaponAction : MonoBehaviour
{
    public Transform target;
    public BodyParts BodyParts;
    public float strength = 1f;
    public bool activate = false;
    public bool done = true;
    public bool deactivateOnDone = true;
    
    

    public virtual void setUpAction(BodyParts bodyParts)
    {
        BodyParts = bodyParts;
    }

    void FixedUpdate()
    {
        if (activate)
        {
            done = false;
            TakeAction();
            if (done && deactivateOnDone) activate = false;
        }
    }

    protected abstract void TakeAction();
}