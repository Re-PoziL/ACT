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

    //�����߼���ÿ�����ܵ��߼���һ��
    public abstract void AbilityLogic();
    

    //��������ʱ����
    protected void AbilityUsed()
    {
        animator.Play(abilityName,0,0);
        abilityCanUse = false;
        GameObjectPool.Instance.SpawnObject("Timer").GetComponent<Timer>().CreatTimer(abilityCDTime, () => abilityCanUse = true);
        Debug.Log("����ʹ����");
    }

    public void ResetAbility()
    {
        abilityCanUse = true;
    }

    
}
