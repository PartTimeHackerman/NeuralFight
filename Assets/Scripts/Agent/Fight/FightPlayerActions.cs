using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FightPlayerActions : MonoBehaviour
{
    public int actionsSpace = 5;
    public HandAction RightHandAction;
    public HandAction LeftHandAction;

    public LocomotionAction Stand;
    public LocomotionAction WalkFW;
    public LocomotionAction WalkBW;
    
    public List<LocomotionAction> LocomotionActions = new List<LocomotionAction>();

    void Start()
    {
        LocomotionActions.Add(Stand);
        LocomotionActions.Add(WalkFW);
        LocomotionActions.Add(WalkBW);
    }
    public void setActions(List<float> actions)
    {
        if (actions.Count != 5)
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
        
        setLocomotionActions(new List<float>(actions).GetRange(2, actions.Count - 1).ToList());
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