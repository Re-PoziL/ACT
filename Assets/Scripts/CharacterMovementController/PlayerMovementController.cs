using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : CharacterMovementBase
{
    [SerializeField] private CameraController _cameraController;

    [Header("��ɫ��С")]
    [SerializeField] private float _characterHeight = 1.8f;
    [SerializeField] private float _characterHeightInCrouch = 1.2f;
    [SerializeField] private Vector3 _characterCenter = new Vector3(0, 0.9f, 0);
    [SerializeField] private Vector3 _characterCenterInCrouch = new Vector3(0, 0.45f, 0);

    [Header("�ƶ��ٶ�")]
    [SerializeField, Range(0, 5)] private float _walkSpeed = 1.5f;
    [SerializeField, Range(0, 15)] private float _runSpeed = 10f;
    [SerializeField, Range(0, 5)] private float _crouchWalkSpeed = 1f;
    [SerializeField, Range(0, 15)] private float _crouchRunSpeed = 5f;
    //��ת���β�ֵʱ��
    [SerializeField, Range(0, 15)] private float _rotationSlerpTime;
    //�ƶ��������β�ֵʱ��
    [SerializeField] private float _moveDirctionSlerpTime = 15f;

    [Header("��������")]
    [SerializeField][Range(1,10)] private float rollSpeedMuti;


    private float _currentMoveSpeed;
    
    private bool _isCrouch = false;


    protected override void Update()
    {
        base.Update();
        UpdatePosition(inputSystem.movePosition);
        UpdateRotation(inputSystem.movePosition);
        UpdateRollMotion();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        AdjustCharacter();
        AdjustMoveSpeed();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
        UpdateCrouchAnimation();
        UpdateMovementAnimation();
        UpdateRollAnimation();
    }

    //���������С
    private void AdjustCharacter()
    {
        if (_isCrouch)
        {
            characterController.center = _characterCenterInCrouch;
            characterController.height = _characterHeightInCrouch;
        }
        else
        {
            characterController.center = _characterCenter;
            characterController.height = _characterHeight;
        }

    }

    //�����ƶ��ٶ�
    private void AdjustMoveSpeed()
    {
        if (_isCrouch)
        {
            _currentMoveSpeed = CanRun() ? _crouchRunSpeed : _crouchWalkSpeed;
        }
        else
        {
            _currentMoveSpeed = CanRun() ? _runSpeed : _walkSpeed;
        }
    }

    #region UpdateAnimation
    private void UpdateCrouchAnimation()
    {
        if (inputSystem.Crouch)
        {
            _isCrouch = !_isCrouch;
            animator.SetBool(anim_CrouchId, _isCrouch);
        }

    }

    private void UpdateMovementAnimation()
    {

        animator.SetFloat(anim_MovementId, inputSystem.movePosition.magnitude * (CanRun() ? 2 : 1), 0.1f, Time.deltaTime);
        animator.SetFloat(anim_RunId, CanRun() ? 1 : 0);


    }

    private void UpdateRollAnimation()
    {
        if (inputSystem.Roll)
        {
            animator.SetTrigger(anim_RollId);
        }
    }
    #endregion

    #region ����

    private bool CanRun()
    {
        return inputSystem.Run;
    }

    private bool CanMove()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsTag(animTag_Motion);
    }
    private bool CanCrouch()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsTag(animTag_Crouch);
    }

    #endregion
    

    private void UpdateRollMotion()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag(animTag_Roll))
        {
            characterController.Move(transform.forward * rollSpeedMuti * animator.GetFloat(anim_AnimationMoveId) * Time.deltaTime);
        }
    }

    private void UpdatePosition(Vector2 moveDirection)
    {
        //����move�������߲���crouch�����оͲ����ƶ�
        if (!CanMove() && !CanCrouch())
        {
            return;
        }
        //�������������
        Quaternion rot = Quaternion.Euler(0, _cameraController.yaw, 0);
        //����������������ƶ�
        Vector3 cameraDir = rot * Vector3.forward * moveDirection.y + rot * Vector3.right * moveDirection.x;
        currentMoveDirection = Vector3.Slerp(currentMoveDirection, ResetMoveDirOnSlope(cameraDir), _moveDirctionSlerpTime * Time.deltaTime);

        characterController.Move(_currentMoveSpeed * Time.deltaTime * moveDirection.magnitude * currentMoveDirection + currentVelocity * Time.deltaTime);


    }

    private void UpdateRotation(Vector2 moveDirection)
    {
        if (!CanMove() && !CanCrouch())
        {
            return;
        }
        if (moveDirection != Vector2.zero)
        {
            //transform.rotation = rot;
            //�漰��ת��ʱ������Ԫ��������ŷ����,gan������������������������������
            float targetRotation = Mathf.Atan2(moveDirection.x, moveDirection.y) * Mathf.Rad2Deg + _cameraController.transform.localEulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0, targetRotation, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _rotationSlerpTime * Time.deltaTime);
        }
    }
}
