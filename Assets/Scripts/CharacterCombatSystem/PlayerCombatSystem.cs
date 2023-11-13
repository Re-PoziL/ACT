using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatSystem : CharacterCombatSystemBase
{

    bool sword = false;

    [SerializeField] private Transform currentTarget;
    [SerializeField] private float detectionRadious;
    private Collider[] targetCollider = new Collider[1];
    [SerializeField] private LayerMask enemyLayer;

    private PlayerHealthSystem playerHealthSystem;
    public override void Awake()
    {
        base.Awake();
        playerHealthSystem = characterController.GetComponent<PlayerHealthSystem>();

    }

    public void Update()
    {
        DetectionTarget();
        AttackMotion();
        UpdateAttackAnimation();
        UpdateCurrentTarget();
        ParryInput();
        //UpdateParryAnimation();
    }

    public void FixedUpdate()
    {

    }

    private void LateUpdate()
    {
        AttackOnLock();
    }

    private void AttackMotion()
    {

        if (animator.GetCurrentAnimatorStateInfo(0).IsTag(attackTag))
        {
            characterController.MoveDirection(transform.forward, animator.GetFloat(animationMove_AnimId) * attackMoveSpeedMultiply);
        }
        
    }

    private void AttackOnLock()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag(attackTag))
        {
            if (!currentTarget)
                return;
            Vector3 rot = currentTarget.position - transform.root.position;
            rot.y = 0;
            Quaternion quaternion = Quaternion.LookRotation(rot);
            transform.root.rotation = Quaternion.Slerp(transform.root.rotation, quaternion, 50f * Time.deltaTime);
        }
    }

    private void UpdateAttackAnimation()
    {
        
        if (inputSystem.LAtk)
        {
            animator.SetTrigger(LAtk_AnimId);

        }
        if(inputSystem.RAtk)
        {
            animator.SetTrigger(RAtk_AnimId);
            animator.SetBool(Sword_AnimId, sword = !sword);
        }
    }


    private void ParryInput()
    {
        if (playerHealthSystem.GetCanExecute() && inputSystem.Parry)
        {
            animator.Play("Execute");
            GameAssets.Instance.PlayAudio(audioSource, AudioSourceType.Hit);
        }
        else
        {
            animator.SetBool(Parry_AnimId, inputSystem.Parry);
        }
    }


    private void UpdateParryAnimation()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).IsTag("hit") && animator.GetCurrentAnimatorStateInfo(0).length>=0.1f)
        {
            if(inputSystem.Parry)
            {
                animator.SetBool(Parry_AnimId, true);
            }
        }
    }

    

    #region Target
    private void DetectionTarget()
    {
        int target = Physics.OverlapSphereNonAlloc(combatDetectionCenter.position, detectionRadious, targetCollider,enemyLayer);
        if(target != 0)
        {
            SetCurrentTarget(targetCollider[0].transform);
        }

    }
   
    private void SetCurrentTarget(Transform target)
    {
        if(currentTarget == null || currentTarget != target)
        {
            currentTarget = target;
        }
    }

    private void UpdateCurrentTarget()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).IsTag(motionTag))
        {
            if(inputSystem.movePosition.sqrMagnitude > 0)
                currentTarget = null;
        }
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(combatDetectionCenter.position, detectionRadious);
    }

}
