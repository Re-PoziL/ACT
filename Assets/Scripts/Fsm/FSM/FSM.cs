using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    public TransitionBaseSO transition;
    [SerializeField] private StateBaseSO currentState;


    public StateBaseSO GetCurrentState() => (currentState);
    public StateBaseSO SetCurrentState(StateBaseSO stateBase) => (currentState = stateBase);


    public void Awake()
    {
        transition?.Init(this);
        currentState?.Init(this);
    }

    private void Start()
    {
        currentState?.OnEnter();
    }

    public void Update()
    {
        transition.TryGetCondition();
        currentState?.OnUpdate();
    }
}
