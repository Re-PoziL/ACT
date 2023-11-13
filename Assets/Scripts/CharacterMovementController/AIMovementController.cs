using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovementController : CharacterMovementBase
{

    protected override void Update()
    {
        base.Update();
        UpdateGravity();
        UpdateRollAnimation();
    }

    private void UpdateRollAnimation()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Roll"))
        {
            MoveDirection(transform.forward, animator.GetFloat(anim_AnimationMoveId));
        }
    }


    private void UpdateGravity()
    {
        characterController.Move(currentVelocity * Time.deltaTime);
    }

}

