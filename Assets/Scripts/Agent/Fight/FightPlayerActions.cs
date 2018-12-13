using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FightPlayerActions : MonoBehaviour
{
    public int actionsSpace = 1;
    private HandAction RightHandAction;
    private HandAction LeftHandAction;
    public BodyParts EnemyBodyParts;

    public LocomotionAction Stand;
    public LocomotionAction Crouch;
    public LocomotionAction WalkFW;
    public LocomotionAction WalkBW;
    public LocomotionAction Nothing;
    
    public List<LocomotionAction> LocomotionActions = new List<LocomotionAction>();
    public LocomotyionType currentAction = LocomotyionType.STAND;
    void Start()
    {
        LocomotionActions.Add(Stand);
        LocomotionActions.Add(Crouch);
        LocomotionActions.Add(WalkFW);
        LocomotionActions.Add(WalkBW);
        LocomotionActions.Add(Nothing);
    }
    
    public void SetUp(FightPlayerAgent player, FightPlayerAgent enemy)
    {
        RightHandAction = player.PlayerFighter.RightArmWeapon.HandAction;
        LeftHandAction = player.PlayerFighter.LeftArmWeapon.HandAction;
        EnemyBodyParts = enemy.PlayerFighter.BodyParts;
    }
    
    public void setActions(List<float> actions)
    {
        if (actions.Count != actionsSpace)
        {
            Debug.Log("Wrong actions size " + actions.Count);
            return;
        }
        
        /*

        if (actions[0] > 0f)
        {
            //RightHandAction.Attack();
        }
        
        if (actions[1] > 0f)
        {
            //LeftHandAction.Attack();
        }

        //RightHandAction.Target = PartsMethods.GetTransformFromAction(actions[2], EnemyBodyParts, WeaponHand.RIGHT);
        //LeftHandAction.Target = PartsMethods.GetTransformFromAction(actions[3], EnemyBodyParts, WeaponHand.LEFT);
        */
        setLocomotionActions(actions[0]);
        //setLocomotionActions(actions);
    }
    private void  setLocomotionActions(float value)
    {
        float normVal = (value + 1f) * .5f;
        normVal *= 3f;

        int intVal = (int)Mathf.Round(normVal);
    
        currentAction = (LocomotyionType)intVal;
        for (int i = 0; i < 4; i++)
        {
            LocomotionActions[i].run = LocomotionActions[i].LocomotyionType == currentAction;
        }
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