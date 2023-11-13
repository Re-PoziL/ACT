using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class SkillChangeConditionSO : ScriptableObject
{
    protected CharacterCombatSystemBase combatSystem;
    protected CharacterHealthSystem healthSystem;

    public virtual bool ConditionSetUp()
    {
        return true;
    }

    public virtual bool ConditionSetUp(Transform owner,Transform target,float distance)
    {
        return Vector3.Distance(owner.position, target.position) <= distance;
            
    }

    public virtual bool ConditionSetUp(bool whether)
    {
        return whether;
    }

    
}
