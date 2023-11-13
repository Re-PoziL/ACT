using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterMovementBase : MonoBehaviour
{
    
    protected Animator animator;
    protected InputSystem inputSystem;
    protected CharacterController characterController;
    protected AudioSource audioSource;

    [SerializeField] protected float gravity = -9.8f;
    [SerializeField] protected Vector3 currentVelocity = Vector3.zero;


    [Header("������")]
    [SerializeField] protected bool isOnGround;
    [SerializeField] protected LayerMask groundLayerMask;
    [SerializeField, Range(-5, 5)] protected float groundDetectionOffset;
    [SerializeField, Range(0, 3)] protected float groundCheckRadius;

    [Header("�¶ȼ��")]
    [SerializeField] protected float slopeCheckLength = 5f;
    
    
    protected Vector3 currentMoveDirection;



    //anim_parameters

    protected int anim_DirectionXId = Animator.StringToHash("DirectionX");
    protected int anim_DirectionYId = Animator.StringToHash("DirectionY");
    protected int anim_RunId = Animator.StringToHash("Run");
    protected int anim_MovementId = Animator.StringToHash("Movement");
    protected int anim_CrouchId = Animator.StringToHash("Crouch");
    protected int anim_RollId = Animator.StringToHash("Roll");
    protected int anim_AnimationMoveId = Animator.StringToHash("AnimationMove");
    //anim_tags
    protected string animTag_Motion = "Motion";
    protected string animTag_Crouch = "Crouch";
    protected string animTag_Roll = "Roll";


    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        inputSystem = GetComponent<InputSystem>();
        characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {

    }


    protected virtual void FixedUpdate()
    {
        CheckOnGround();

    }

    protected virtual void Update()
    {

        AdjustVelocity();    
    }

    protected virtual void LateUpdate()
    {
        
    }

    //���������ٶ�
    private void AdjustVelocity()
    {
        if (isOnGround)
        {
            currentVelocity.y = 0f;
        }
        currentVelocity.y += gravity * Time.deltaTime;

    }

    

    private void CheckOnGround()
    {
        Vector3 groundCheckPosition = transform.position;
        groundCheckPosition.y -= groundDetectionOffset;
        isOnGround = Physics.CheckSphere(groundCheckPosition, groundCheckRadius, groundLayerMask);
    }


    //�¶ȼ��
    protected Vector3 ResetMoveDirOnSlope(Vector3 dir)
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitinfo, slopeCheckLength))
        {
            float newAngle = Vector3.Dot(Vector3.up, hitinfo.normal);

            if (newAngle != 1 && currentVelocity.y <= 0)
            {
                return Vector3.ProjectOnPlane(dir, hitinfo.normal);
            }
        }
        return dir;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + Vector3.down * groundDetectionOffset, groundCheckRadius);

    }


    #region ��������
    /// <summary>
    /// �ƶ�
    /// </summary>
    /// <param name="moveDir">�ƶ�����</param>
    /// <param name="speed">�ƶ��ٶ�</param>
    public void MoveDirection(Vector3 moveDir, float speed, bool useGravity = true)
    {
        if (!useGravity)
        {
            currentVelocity = Vector3.zero;
        }
        currentMoveDirection = moveDir.normalized;
        characterController.Move(currentMoveDirection * speed * Time.deltaTime + currentVelocity * Time.deltaTime);
    }


    #endregion
}
