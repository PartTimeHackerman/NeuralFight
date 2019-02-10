using System.Collections.Generic;
using UnityEngine;

public class Block : OneHandedAction
 {
     public float upperArmAng = -45f;
     public float lowerArmAng = 125f;


     protected override void TakeAction()
     {
         float rotation = getRotation();
         float upperRot = Mathf.Clamp(-rotation - upperArmAng, upperArm.angularLimits[0][0], upperArm.angularLimits[0][1]);
         float lowerRot = lowerArmAng;
         upperArm.SetConfigurableForceAndRot(strength, new Vector3(upperRot, 0f, 0f));
         lowerArm.SetConfigurableForceAndRot(strength, new Vector3(lowerRot, 0f, 0f));
         done = true;
     }
 }