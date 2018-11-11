using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FightPlayerActions : MonoBehaviour
{
    public int actionsSpace = 9;
    public HandAction RightHandAction;
    public HandAction LeftHandAction;
    public BodyParts EnemyBodyParts;

    public LocomotionAction Stand;
    public LocomotionAction Crouch;
    public LocomotionAction WalkFW;
    public LocomotionAction WalkBW;
    public LocomotionAction Nothing;
    
    public List<LocomotionAction> LocomotionActions = new List<LocomotionAction>();

    void Start()
    {
        LocomotionActions.Add(Stand);
        LocomotionActions.Add(Crouch);
        LocomotionActions.Add(WalkFW);
        LocomotionActions.Add(WalkBW);
        LocomotionActions.Add(Nothing);
    }
    public void setActions(List<float> actions)
    {
        if (actions.Count != actionsSpace)
        {
            Debug.Log("Wrong actions size " + actions.Count);
            return;
        }

        if (actions[0] > 0f)
        {
            RightHandAction.Attack();
        }
        
        if (actions[1] > 0f)
        {
            LeftHandAction.Attack();
        }

        RightHandAction.Target = PartsMethods.GetTransformFromAction(actions[2], EnemyBodyParts);
        LeftHandAction.Target = PartsMethods.GetTransformFromAction(actions[3], EnemyBodyParts);
        
        setLocomotionActions(new List<float>(actions).GetRange(4, 4).ToList());
    }
    
    private void setLocomotionActions(List<float> actions)
    {
        int highestAction = 0;
        float hidhestVal = -1f;

        for (int i = 0; i < actions.Count; i++)
        {
            float actionVal = actions[i];
            if (actionVal > hidhestVal)
            {
                hidhestVal = actionVal;
                highestAction = i;
            }
        }

        for (int i = 0; i < LocomotionActions.Count; i++)
        {
            LocomotionActions[i].run = i == highestAction;
        }
        

    }

}