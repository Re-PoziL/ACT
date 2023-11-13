 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealthSystem : CharacterHealthSystem
{

    [SerializeField] private int maxParryCount = 5;
    [SerializeField] private int currentParryCount = 0;
    [SerializeField] private int maxHitCount=5;
    [SerializeField] private int currentHitCount=0;
    [SerializeField] private int strikeBackCount=3;



    private void LateUpdate()
    {
        FaceAttacker();
    }

    public override void TakeDamage(float damagar, string hitAnimationName, Transform attacker)
    {
        if (hitAnimationName.Equals("Executed"))
        {
            animator.Play(hitAnimationName);
            GameAssets.Instance.PlayAudio(audioSource, AudioSourceType.Hit);
            return;
        }
        if (currentParryCount == strikeBackCount)
        {
            
            animator.Play("Attack01", 0, 0);
            GameAssets.Instance.PlayAudio(audioSource, AudioSourceType.Parry);
            currentParryCount++;
            return;
        }
        if (currentParryCount < maxParryCount)
        {
            Parry(hitAnimationName);
            currentParryCount++;
            if(currentParryCount >= maxParryCount)
            {
                GameObjectPool.Instance.SpawnObject("Timer").GetComponent<Timer>().CreatTimer(10f, () => currentParryCount = 0);
            }
        }
        else
        {
            if (currentHitCount >= maxHitCount)
            {
                currentHitCount = 0;
                animator.Play("Roll", 0, 0);
            }
            else
            {
                animator.Play(hitAnimationName);
                GameAssets.Instance.PlayAudio(audioSource, AudioSourceType.Hit);
                currentHitCount++;
            }
        }
    }


    private void Parry(string hitAnimationName)
    {
        switch (hitAnimationName)
        {
            case "Hit_H_Right":
                animator.Play("ParryF", 0, 0);
                GameAssets.Instance.PlayAudio(audioSource, AudioSourceType.Parry);
                break;
            case "Hit_D_Up":
                animator.Play("ParryL", 0, 0);
                GameAssets.Instance.PlayAudio(audioSource, AudioSourceType.Parry);
                break;
            case "Hit_Up_Left":
                animator.Play("ParryR", 0, 0);
                GameAssets.Instance.PlayAudio(audioSource, AudioSourceType.Parry);
                break;
            default:
                animator.Play("ParryL", 0, 0);
                GameAssets.Instance.PlayAudio(audioSource, AudioSourceType.Parry);
                break;
        }

    }

    private void FaceAttacker()
    {
        if (!currentAttacker)
            return;
        if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("Hit"))
            return;
        Vector3 direction = currentAttacker.position - transform.position;
        direction.y = 0f;
        Quaternion quaternion = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, quaternion, 50f);
    }

}
