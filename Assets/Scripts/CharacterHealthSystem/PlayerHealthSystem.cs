using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthSystem : CharacterHealthSystem
{
    private bool execute = false;
    private void LateUpdate()
    {
        FaceAttacker();
    }

    private bool CanParry()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Parry"))
        {
            return true;
        }
        return false;

    }

    public bool GetCanExecute() => execute;

    public override void TakeDamage(float damagar, string hitAnimationName, Transform attacker)
    {
        //触发完美弹反
        if (CanParry())
        {
            if (GetCanPerfectFlick())
            {
                attacker.GetComponent<Animator>().Play("Flick");
                Time.timeScale = 0.25f;
                execute = true;
                GameObjectPool.Instance.SpawnObject("Timer").GetComponent<Timer>().CreatTimer(0.1f, () =>
                {
                    Time.timeScale = 1;
                    execute = false;
                });
            }
            else
            {
                Parry(hitAnimationName);
                GameAssets.Instance.PlayAudio(audioSource, AudioSourceType.Parry);
            }
        }
        else
        {
            animator.Play(hitAnimationName);
            GameAssets.Instance.PlayAudio(audioSource, AudioSourceType.Hit);
        }
    }

    private void Parry(string hitAnimationName)
    {
        switch (hitAnimationName)
        {
            case "Hit_D_Up":
                animator.Play("ParryF", 0, 0);
                break;
            case "Hit_H_Right":
                animator.Play("ParryR", 0, 0);
                break;
            case "Hit_Up_Left":
                animator.Play("ParryL", 0, 0);
                break;
            default:
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
