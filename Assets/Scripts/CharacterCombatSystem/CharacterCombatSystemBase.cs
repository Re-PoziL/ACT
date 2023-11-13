using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterCombatSystemBase : MonoBehaviour
{
    protected Animator animator;
    protected InputSystem inputSystem;
    protected CharacterMovementBase characterController;
    protected AudioSource audioSource;
    private bool canPerFlick = false;
    //用于检测攻击范围
    [SerializeField]protected Transform combatDetectionCenter;
    [SerializeField][Range(1,10)]protected float combatRadius;
    [SerializeField, Range(0, 10)] protected float attackMoveSpeedMultiply;

    //animation parameter
    protected int LAtk_AnimId = Animator.StringToHash("LAtk");
    protected int RAtk_AnimId = Animator.StringToHash("RAtk");
    protected int Sword_AnimId = Animator.StringToHash("Sword");
    protected int Parry_AnimId = Animator.StringToHash("Parry");
    protected int animationMove_AnimId = Animator.StringToHash("AnimationMove");

    protected string attackTag = "Attack";
    protected string motionTag = "Motion";
    protected string parryTag = "Parry";


    private AudioSourceType audioSourceType = AudioSourceType.Null;
    public virtual void Awake()
    {

        animator = GetComponent<Animator>();
        characterController = GetComponentInParent<CharacterMovementBase>();
        inputSystem = characterController.GetComponent<InputSystem>();
        audioSource = characterController.GetComponent<AudioSource>();
    }


    private void OnDrawGizmos()
    {
        
    }


    public void OnAnimationAttackEvent(string hitName)
    {
        Collider[] colliders = new Collider[4];
        int result = Physics.OverlapSphereNonAlloc(combatDetectionCenter.position,combatRadius, colliders);
        if(result != 0)
        {
            if (colliders[0] == gameObject)
                return;
            for (int i = 0; i < result; i++)
            {
                if(colliders[i].TryGetComponent(out IDamagar damager))
                {
                    damager.TakeDamage(0,hitName,transform.transform);
                }
            }
        }
        GameAssets.Instance.PlayAudio(characterController.GetComponent<AudioSource>(), audioSourceType);
    }

    public void OnAnimationAudiovent(string audioType)
    {
        switch (audioType)
        {
            case "Katana":
                audioSourceType = AudioSourceType.Katana;
                break;
            case "GreatSword":
                audioSourceType = AudioSourceType.GreatSword;
                break;
        }
    }

    //完美弹反检测开始
    public void OnFlickPerfectStartEvent()
    {
        canPerFlick = true;
    }

    public void OnFlickPerfectEndEvent()
    {
        canPerFlick = false;
    }

    public bool GetCanPerfectFlick() => canPerFlick;
}
