using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagar
{
    //��ʽ����
    void TakeDamage(float damager);
    void TakeDamage(string hitAnimationName);
    void TakeDamage(float damager, string hitAnimationName);
    void TakeDamage(float damagar, string hitAnimationName, Transform attacker);


}
