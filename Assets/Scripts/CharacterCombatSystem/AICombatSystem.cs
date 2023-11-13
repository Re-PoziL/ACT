using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICombatSystem : CharacterCombatSystemBase
{
    [SerializeField] private Transform detectionCenter;
    [SerializeField] [Range(0, 10)] private float detectionRange;
    [SerializeField] private Transform currentTarget;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private List<CombatAbilityBase> abilitys = new List<CombatAbilityBase>();
    private Collider[] colliders = new Collider[1];


    private AIMovementController aIMovementController;

    public override void Awake()
    {
        base.Awake();
        aIMovementController = GetComponentInParent<AIMovementController>();
    }


    private void Update()
    {
        AIView();
    }

    private void Start()
    {
        InitAbility();
    }

    private void InitAbility()
    {
        if (abilitys.Count == 0)
            return;
        for (int i = 0; i < abilitys.Count; i++)
        {
            abilitys[i].Init(this, animator, aIMovementController);
            abilitys[i].ResetAbility();
        }
    }
    
    public CombatAbilityBase GetOneAbility_CanUse()
    {
        for (int i = 0; i < abilitys.Count; i++)
        {
            if(abilitys[i].abilityCanUse)
            {
                return abilitys[i];
            }
        }
        return null;
    }

    public CombatAbilityBase GetAbilityByID(int id)
    {
        for (int i = 0; i < abilitys.Count; i++)
        {
            if (abilitys[i].abilityID == id)
            {
                return abilitys[i];
            }
        }
        return null;
    }

    public CombatAbilityBase GetAbilityByName(string name)
    {
        for (int i = 0; i < abilitys.Count; i++)
        {
            if (abilitys[i].abilityName.Equals(name))
            {
                return abilitys[i];
            }
        }
        return null;
    }

    private void AIView()
    {
        //先检测有没有角色进入视野范围，如果有，检测有没有障碍物遮挡，最后检测视野角度
        int res = Physics.OverlapSphereNonAlloc(detectionCenter.position, detectionRange, colliders, enemyLayer);
        if (res != 0)
        {
            if (!Physics.Raycast(transform.root.position + Vector3.up * .5f, colliders[0].transform.position - transform.root.position, detectionRange, obstacleLayer))
            {
                if (Vector3.Dot(transform.root.forward.normalized, (colliders[0].transform.position.normalized - detectionCenter.position.normalized).normalized) > 0.35f)
                {
                    SetTarget(colliders[0].transform);
                }
            }

        }
    }

    private void SetTarget(Transform target)
    {
        if (currentTarget == null || currentTarget != target)
        {
            currentTarget = target;
        }
    }

    public Transform GetTarget()
    {
        return currentTarget;
    }

    public float GetTargetDistance()
    {
        if(currentTarget!=null)
            return Vector3.Distance(transform.root.position,currentTarget.position);
        return 0f;
    }

    public Vector3 GetTargetForward()
    {
        if (currentTarget != null)
        {
            Vector3 forward = currentTarget.position - aIMovementController.transform.position;
            return forward;

        }
        return aIMovementController.transform.forward;
    }

    public void SetRotation()
    {
        Vector3 direction = currentTarget.position - aIMovementController.transform.position;
        direction.y = 0f;
        Quaternion quaternion = Quaternion.LookRotation(direction);
        aIMovementController.transform.rotation = Quaternion.Slerp(aIMovementController.transform.rotation, quaternion, 0.2f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(detectionCenter.position, detectionRange);
        Gizmos.DrawWireSphere(combatDetectionCenter.position, combatRadius);
    }
}
