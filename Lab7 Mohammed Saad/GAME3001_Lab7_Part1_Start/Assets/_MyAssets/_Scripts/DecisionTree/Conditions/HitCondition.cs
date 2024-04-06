using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCondition : ConditionNode
{
    public bool IsHit { get; set; }

    public HitCondition()
    {
        name = "Ranged Combat Condition";
        IsHit = false;
    }

    public override bool Condition()
    {
        Debug.Log("Checking " + name);
        return IsHit;
    }
}
