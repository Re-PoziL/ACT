using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="SkillCondition1",menuName = "SkillCondition/SkillCondition1")]
public class SkillCondition1 : SkillChangeConditionSO
{

    public override bool ConditionSetUp(Transform owner, Transform target, float distance)
    {
        return base.ConditionSetUp(owner, target, distance);
    }
}
