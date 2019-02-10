using DG.Tweening;
using UnityEngine;

public class Treadmill : MonoBehaviour
{
    public Transform Begin;
    public Transform End;
    public Fighter Fighter;
    public bool run = false;

    public void SetFighter(Fighter fighter)
    {
        Fighter = fighter;
        Fighter.RightArmWeapon.HandAction.Target = End;
        Fighter.LeftArmWeapon.HandAction.Target = End;
        Fighter.BodyParts.SetEnableJoints(true);
        Fighter.BodyParts.SetKinematic(false);
        Fighter.RightArmWeapon.HandAction.setActive(true);
        Fighter.LeftArmWeapon.HandAction.setActive(true);
        Fighter.LocomotionActions.Find(a => a.LocomotyionType == LocomotyionType.WALK_FORWARD).run = true;
        Fighter.BodyParts.root.transform.position = ObjectsPositions.TreadmillFighterPos;
        run = true;
    }

    public void Stop()
    {
        run = false;
        if (Fighter != null)
        {
            Fighter.ResetFighter();
        }
    }

    private void FixedUpdate()
    {
        if (run)
        {
            if (Fighter.BodyParts.root.transform.position.x >= End.position.x)
            {
                Vector3 newPos = Fighter.BodyParts.root.transform.position;
                newPos.x = Begin.position.x;
                Fighter.BodyParts.root.transform.position = newPos;
            }
        }
    }
}