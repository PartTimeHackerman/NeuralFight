using System;
using System.Collections.Generic;
using UnityEngine;

public class TwoHandedAction<T> : WeaponAction where T : WeaponAction
{
    protected Rigidbody rUpperArm;
    protected Rigidbody lUpperArm;
    protected T rArmAction;
    protected T lArmAction;
    protected Action setAction = () => {};

    public override void setUpAction(BodyParts bodyParts)
    {
        base.setUpAction(bodyParts);
        foreach (KeyValuePair<string, Rigidbody> keyValuePair in BodyParts.getNamedRigids())
        {
            if (keyValuePair.Key.Contains("rupper")) rUpperArm = keyValuePair.Value;
            if (keyValuePair.Key.Contains("lupper")) lUpperArm = keyValuePair.Value;
        }

        addBodyParts(rUpperArm.gameObject);
        addBodyParts(lUpperArm.gameObject);
        
        rArmAction = rUpperArm.gameObject.AddComponent<T>();
        lArmAction = lUpperArm.gameObject.AddComponent<T>();
        rArmAction.target = target;
        lArmAction.target = target;
        setAction();
    }

    private void addBodyParts(GameObject gameObject)
    {
        if (!gameObject.GetComponent<BodyParts>())
        {
            BodyParts BodyParts = gameObject.AddComponent<BodyParts>();
            BodyParts.root = this.BodyParts.root;
        }
    }


    protected override void TakeAction()
    {
        rArmAction.activate = activate;
        lArmAction.activate = activate;
        done = rArmAction.done || lArmAction.done;
    }
}