using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystem : MonoBehaviour
{

    //inputGameAction
    private MyGameAction m_GameAction;


    //keyBinding
    public Vector2 movePosition
    {
        get => m_GameAction.Player.Move.ReadValue<Vector2>();
    }

    public Vector2 lookDelta
    {
        get => m_GameAction.Player.Look.ReadValue<Vector2>();
        
    }

    public bool Run
    {
        get => m_GameAction.Player.Run.phase == InputActionPhase.Performed;
    }
    
    public bool LAtk 
    {
        get => m_GameAction.Player.LAtk.triggered;
    }

    public bool RAtk
    {
        get => m_GameAction.Player.RAtk.triggered;
    }


    public bool Roll
    {
        get => m_GameAction.Player.Roll.triggered;
    }

    public bool Parry
    {
        get => m_GameAction.Player.Parry.phase == InputActionPhase.Performed;
    }

    public bool Crouch
    {
        get => m_GameAction.Player.Crouch.WasPressedThisFrame();
    }


    private void Awake()
    {
        if (m_GameAction == null)
            m_GameAction = new MyGameAction();
    }


    private void OnEnable()
    {
        m_GameAction.Player.Enable();
    }

    private void OnDisable()
    {
        m_GameAction.Player.Disable();
    }
}
