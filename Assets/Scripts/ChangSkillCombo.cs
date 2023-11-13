using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangSkillCombo : StateMachineBehaviour
{
    public string changeSkillName;
    public float changeTime;
    public SkillChangeConditionSO skillChangeCondition;

    private AICombatSystem combatSystem;


    private int changeSkillHash;
    private bool allowChange = false;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        combatSystem = animator.GetComponent<AICombatSystem>();
        Debug.Log(combatSystem);

        changeSkillHash = Animator.StringToHash(changeSkillName);
    }

    
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //在允许变招的时间满足变招条件
        if(stateInfo.normalizedTime <= changeTime)
        {
            if (skillChangeCondition.ConditionSetUp(animator.transform, combatSystem.GetTarget(), 3f))
            {
                allowChange = true;
            }
        }
        
        //在允许变招时间结束后并且允许变招
        if(allowChange && stateInfo.normalizedTime > changeTime)
        {
            animator.CrossFade(changeSkillHash,0,0,0);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        allowChange = false;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}


}
