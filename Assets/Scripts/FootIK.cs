using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootIK : MonoBehaviour
{
    private Animator _animator;
    [SerializeField,Range(0,1)]private float rightFootweight;
    [SerializeField, Range(0, 1)] private float leftFootweight;
    
    private float weight;

    private Vector3 rightFootIK;
    
    private Vector3 leftFootIK;


    [Range(0,1)]public float groundOffset;


    private string anim_WeightId = "Weight";
    public LayerMask groundLayer;
    private void Awake()
    {
        _animator = GetComponent<Animator>();

        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private Quaternion AdjustFootIKRotation(Vector3 foot)
    {
        if (Physics.Raycast(foot + Vector3.down * 0.1f, Vector3.down, out RaycastHit hitinfo, .5f, groundLayer))
        {
            
            return Quaternion.Slerp(transform.rotation,Quaternion.FromToRotation(Vector3.up, hitinfo.normal) * transform.rotation,0.5f);

        }
        

        return transform.rotation;
        
    }

    private Vector3 AdjustFootIKPosition(Vector3 foot)
    {
        if(Physics.Raycast(foot, Vector3.down,out RaycastHit hitinfo, .5f, groundLayer))
        {
            return  hitinfo.point + Vector3.up * groundOffset;
        }
        return foot;

    }

    private void OnAnimatorIK(int layerIndex)
    {
        rightFootIK = _animator.GetIKPosition(AvatarIKGoal.RightFoot);
        leftFootIK = _animator.GetIKPosition(AvatarIKGoal.LeftFoot);
        weight = _animator.GetFloat(anim_WeightId);

        if(!CheckOnSlope())
        {
            return;
        }

        _animator.SetIKPosition(AvatarIKGoal.LeftFoot, AdjustFootIKPosition(leftFootIK));
        _animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, weight);
        _animator.SetIKRotation(AvatarIKGoal.LeftFoot, AdjustFootIKRotation(leftFootIK));
        _animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, weight);

        _animator.SetIKPosition(AvatarIKGoal.RightFoot, AdjustFootIKPosition(rightFootIK));
        _animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, weight);
        SetIKPosition(AvatarIKGoal.RightFoot, AdjustFootIKRotation(rightFootIK), weight);
    
    }

    private void SetIKPosition(AvatarIKGoal avatarIKGoal,Quaternion quaternion,float weight)
    {
        _animator.SetIKRotation(avatarIKGoal, quaternion);
        _animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, weight);
    }

    private bool CheckOnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitinfo,1f,groundLayer))
        {
            if(Vector3.Dot(Vector3.up, hitinfo.normal) != 1)
            {
                return true;
            }
        }
        return false;
    }
}
