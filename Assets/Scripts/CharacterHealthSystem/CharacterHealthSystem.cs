using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterHealthSystem : MonoBehaviour,IDamagar
{

    protected Animator animator;
    protected AudioSource audioSource;
    protected CharacterMovementBase characterMovement;
    protected Transform currentAttacker;
    protected CharacterCombatSystemBase characterCombatSystemBase;
    [SerializeField][Range(1,3)] private float Mutil;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        characterMovement = GetComponent<CharacterMovementBase>();
        characterCombatSystemBase = GetComponentInChildren<CharacterCombatSystemBase>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void HitMotion()
    {
        bool hit = animator.GetCurrentAnimatorStateInfo(0).IsTag("Hit");
        if (hit)
        {
            characterMovement.MoveDirection(transform.forward,animator.GetFloat("AnimationMove") * Mutil);
        }

    }

    protected bool GetCanPerfectFlick()
    {
        return characterCombatSystemBase.GetCanPerfectFlick();
    }


    public void TakeDamage(float damager)
    {
        throw new System.NotImplementedException();
    }

    public virtual void TakeDamage(string hitAnimationName)
    {
        animator.Play(hitAnimationName);
        HitMotion();
        GameAssets.Instance.PlayAudio(audioSource, AudioSourceType.Hit);
    }

    public virtual void TakeDamage(float damager, string hitAnimationName)
    {
        throw new System.NotImplementedException();
    }

    public virtual void TakeDamage(float damagar, string hitAnimationName, Transform attacker)
    {
        animator.Play(hitAnimationName);
        HitMotion();
        GameAssets.Instance.PlayAudio(audioSource, AudioSourceType.Hit);
        SetCurrentAttacker(attacker);
    }

    private void SetCurrentAttacker(Transform attacker)
    {
        if(currentAttacker == null || currentAttacker != attacker)
        {
            currentAttacker = attacker;
        }
    }
    
}
