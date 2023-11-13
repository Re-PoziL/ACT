using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IdleState", menuName = "FSM/State/IdleState")]
public class IdleState : StateBaseSO
{


    public override void OnEnter()
    {
        Debug.Log("EnterIdle");
    }

    public override void OnExit()
    {
        Debug.Log("ExitIdle");
    }

    public override void OnUpdate()
    {
        Debug.Log("Idle...");
    }

}
