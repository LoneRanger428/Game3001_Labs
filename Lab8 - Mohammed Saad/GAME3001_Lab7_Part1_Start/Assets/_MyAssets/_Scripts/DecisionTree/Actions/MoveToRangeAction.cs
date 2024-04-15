using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToRangeAction : ActionNode
{
    public MoveToRangeAction()
    {
        name = "Move to Range Action";
    }
    public override void Action()
    {
        //Enter functionality for action.
        if (Agent.GetComponent<AgentObject>().state != ActionState.MOVE_TO_RANGE)
        {
            Debug.Log("Starting " + name);
            AgentObject ao = Agent.GetComponent<AgentObject>();
            ao.state = ActionState.MOVE_TO_RANGE;

            //Enter custom actions
            if (AgentScript is RangedCombatEnemy rce)
            {

            }
        }
        //Every frame actions
        Debug.Log("Performing " + name);
    }
}
