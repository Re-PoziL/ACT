using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CombatState", menuName = "FSM/State/CombatState")]
public class CombatState : StateBaseSO
{

    [Range(1, 10)] public float alertDistance;
    [Range(1, 10)] public float closeDistance;
    [Range(1,10)]public float moveSpeed;

    private Transform target;

    //animator Parameters
    private int animId_Movement = Animator.StringToHash("Movement");
    private int animId_Roll = Animator.StringToHash("Roll");
    private int animId_AnimationMove = Animator.StringToHash("AnimationMove");

    [SerializeField] private CombatAbilityBase currentAbility;
    
    public override void OnEnter()
    {
        Debug.Log("EnterCombat");
        target = aICombatSystem.GetTarget();
        currentAbility = aICombatSystem.GetOneAbility_CanUse();
    }

    public override void OnExit()
    {
        throw new System.NotImplementedException();
    }

    public override void OnUpdate()
    {
        if(currentAbility == null)
        {
            OnAlertDistance();
            currentAbility = aICombatSystem.GetOneAbility_CanUse();
        }
        else
        {
            currentAbility.AbilityLogic();
            if(!currentAbility.abilityCanUse)
            {
                currentAbility = null;
            }
        }
        
    }
    

    private void OnAlertDistance()
    {
        if (Mathf.Abs(Vector3.Distance(aIMovementController.transform.position, target.position) - alertDistance) <= .1f)
        {
            animator.SetFloat(animId_Movement, 0f, 0.1f, Time.deltaTime);
            return;
        }
        //如果和目标的距离大于警惕距离，就往前
        if (Vector3.Distance(aIMovementController.transform.position,target.position) > alertDistance)
        {
            aIMovementController.MoveDirection(GetTargetForward(), moveSpeed);
            animator.SetFloat(animId_Movement, .5f);
            SetRotation();
        }
        //反之，就往后
        if(Vector3.Distance(aIMovementController.transform.position, target.position) < alertDistance)
        {
            aIMovementController.MoveDirection(-GetTargetForward(), moveSpeed);
            animator.SetFloat(animId_Movement, .5f);
            SetRotation();
        }
        
    }

    private void SetRotation()
    {
        Vector3 direction = target.position - aIMovementController.transform.position;
        direction.y = 0f;
        Quaternion quaternion = Quaternion.LookRotation(direction);
        aIMovementController.transform.rotation = Quaternion.Slerp(aIMovementController.transform.rotation, quaternion, 0.2f);
    }

    private Vector3 GetTargetForward()
    {
        if(target != null)
        {
            Vector3 forward = target.position - aIMovementController.transform.position;
            return forward;

        }
        return aIMovementController.transform.forward;
    }

    

}
