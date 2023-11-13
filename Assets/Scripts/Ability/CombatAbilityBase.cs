using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatAbilityBase : ScriptableObject
{

    public string abilityName;
    public int abilityID;
    public float abilityCDTime;
    public float abilityUseDistance;
    public bool abilityCanUse;

    protected AICombatSystem aICombatSystem;
    protected Animator animator;
    protected AIMovementController aIMovementController;
    protected int animId_Movement = Animator.StringToHash("Movement");
    protected int animId_AnimationMove = Animator.StringToHash("AnimationMove");

    public void Init(AICombatSystem aICombatSystem,Animator animator,AIMovementController aIMovementController)
    {
        this.aICombatSystem = aICombatSystem;
        this.animator = animator;
        this.aIMovementController = aIMovementController;
    }

    //技能逻辑，每个技能的逻辑不一样
    public abstract void AbilityLogic();
    

    //触发技能时调用
    protected void AbilityUsed()
    {
        animator.Play(abilityName,0,0);
        abilityCanUse = false;
        GameObjectPool.Instance.SpawnObject("Timer").GetComponent<Timer>().CreatTimer(abilityCDTime, () => abilityCanUse = true);
        Debug.Log("技能使用了");
    }

    public void ResetAbility()
    {
        abilityCanUse = true;
    }

    
}
