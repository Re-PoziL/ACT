using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ToCombatCondition", menuName = "FSM/Condition/ToCombatCondition")]
public class ToCombatCondition : ConditionBaseSO
{


    public override bool ConditionSetUp()
    {
        return aICombatSystem.GetTarget() != null;
    }

}
