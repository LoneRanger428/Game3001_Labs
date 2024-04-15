using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : ActionNode
{
    public AttackAction()
    {
        name = "Attack Action";
    }
    public override void Action()
    {
        //Enter functionality for action.
        if(Agent.GetComponent<AgentObject>().state != ActionState.ATTACK) //Calling for the first time
        {
            Debug.Log("Starting " + name);
            AgentObject ao = Agent.GetComponent<AgentObject>();
            ao.state = ActionState.ATTACK;

            //Enter custom actions
            if(AgentScript is CloseCombatEnemy cce)
            {

            }
            else if (AgentScript is RangedCombatEnemy rce)
            {

            }
        }

        //Every frame
        Debug.Log("Performing " + name);
    }
}
