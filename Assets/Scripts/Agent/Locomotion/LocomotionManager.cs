using System;
using System.Collections.Generic;
using UnityEngine;

public class LocomotionManager : MonoBehaviour
{
    public int ActionsNumber = 3;
    public LocomotionAction StandAction;
    public LocomotionAction WalkForwardAction;
    public LocomotionAction WalkBackwardAction;

    public List<LocomotionAction> LocomotionActions = new List<LocomotionAction>();

    void Start()
    {
        LocomotionActions.Add(StandAction);
        LocomotionActions.Add(WalkForwardAction);
        LocomotionActions.Add(WalkBackwardAction);
    }

    public void RunAction(List<float> actions)
    {
        int highestAction = 0;
        float hidhestVal = -1f;
        if (actions.Count != ActionsNumber)
        {
            throw new Exception("Actions counts don't match");
        }

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