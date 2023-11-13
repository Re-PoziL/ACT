using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NormalAbility",menuName ="Ability/NormalAbility")]
public class NormalAbility : CombatAbilityBase
{

    
    public override void AbilityLogic()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).IsTag("Ability"))
        {
            aIMovementController.MoveDirection(aIMovementController.transform.forward, animator.GetFloat(animId_AnimationMove));
        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsTag("Motion") && abilityCanUse)
        {
            if(aICombatSystem.GetTargetDistance() < abilityUseDistance)
            {
                animator.Play(abilityName);
                
                AbilityUsed();
            }
            else
            {
                aIMovementController.MoveDirection(aICombatSystem.GetTargetForward(),2f);
                aICombatSystem.SetRotation();
                animator.SetFloat(animId_Movement,1f);
            }
        }
    }

    

}
