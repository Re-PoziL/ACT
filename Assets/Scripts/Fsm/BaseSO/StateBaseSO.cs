using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//≥ÈœÛ¿‡ºÃ≥–”⁄ScriptableObject
public abstract class StateBaseSO : ScriptableObject
{
    [SerializeField] protected int priority;

    protected FSM currentFsm;
    protected AICombatSystem aICombatSystem;
    protected AIMovementController aIMovementController;
    protected Animator animator;


    public virtual void Init(FSM fSM)
    {
        if (currentFsm == null)
        {
            currentFsm = fSM;
            aICombatSystem = currentFsm.GetComponentInChildren<AICombatSystem>();
            aIMovementController = currentFsm.GetComponent<AIMovementController>();
            animator = currentFsm.GetComponentInChildren<Animator>();
        }
    }

    public int GetPriority() => (priority);

    public abstract void OnEnter();

    public abstract void OnUpdate();

    public abstract void OnExit();

}
