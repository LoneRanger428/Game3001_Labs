using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeAction : ActionNode
{
    public FleeAction()
    {
        name = "Flee Action";

    }
    public override void Action()
    {
        //Enter functionality for action.
        if (Agent.GetComponent<AgentObject>().state != ActionState.FLEE)
        {
            Debug.Log("Starting " + name);
            AgentObject ao = Agent.GetComponent<AgentObject>();
            ao.state = ActionState.FLEE;

            //Enter custom actions
            if (AgentScript is RangedCombatEnemy rce)
            {

            }
        }
        //Every frame actions
        Debug.Log("Performing " + name);
    }
}
