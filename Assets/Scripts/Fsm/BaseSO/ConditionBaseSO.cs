using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ConditionBaseSO : ScriptableObject
{
    public int priority;

    private FSM currentFsm;

    protected AICombatSystem aICombatSystem;

    public virtual void Init(FSM fSM)
    {
        if (currentFsm == null)
        {
            currentFsm = fSM;
            aICombatSystem = fSM.GetComponentInChildren<AICombatSystem>();
        }
    }

    public abstract bool ConditionSetUp();
}
