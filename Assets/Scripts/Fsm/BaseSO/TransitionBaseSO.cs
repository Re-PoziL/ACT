using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "TransitionSO",menuName = "FSM/Transition/Transition")]
public class TransitionBaseSO : ScriptableObject
{
    [System.Serializable]
    public struct StateConfig
    {
        public StateBaseSO fromState;
        public StateBaseSO toState;
        public List<ConditionBaseSO> conditions;
    }


    public List<StateConfig> statesConfig;

    //状态字典里面保存的是前置状态：他的所有转换配置
    private Dictionary<StateBaseSO, List<StateConfig>> stateDic = new Dictionary<StateBaseSO, List<StateConfig>>();


    private FSM currentFsm;
    public void Init(FSM fsm)
    {
        currentFsm = fsm;
        SaveStateConfig();
    }

    private void SaveStateConfig()
    {
        foreach (var item in statesConfig)
        {
            //字典里面没有这个key
            if(!stateDic.ContainsKey(item.fromState))
            {
                //new key value
                stateDic.Add(item.fromState, new List<StateConfig>());
                //add value
                stateDic[item.fromState].Add(item);
                
            }
            else
            {
                stateDic[item.fromState].Add(item);
            }
        }

    }

    public void TryGetCondition()
    {
        int priority = 0;
        List<StateBaseSO> toStates = new List<StateBaseSO>();
        StateBaseSO toState = null;
        foreach (var state in stateDic)
        {
            if(state.Key == currentFsm.GetCurrentState())
            {
                foreach (var item in state.Value)
                {
                    foreach (var condition in item.conditions)
                    {
                        condition.Init(currentFsm);
                        if(condition.ConditionSetUp())
                        {
                            toStates.Add(item.toState);
                            Debug.Log("条件满足");
                        }
                    }
                }
            }
        }

        //有能够转换的条件
        if(toStates.Count != 0)
        {
            foreach (var item in toStates)
            {
                if(item.GetPriority() >= priority)
                {
                    priority = item.GetPriority();
                    toState = item;
                }
            }
            ChangeState(toState);
        }
        
    }

    private void ChangeState(StateBaseSO state)
    {
        currentFsm.GetCurrentState().OnExit();
        currentFsm.SetCurrentState(state);
        //初始化新状态
        currentFsm.GetCurrentState().Init(currentFsm);
        currentFsm.GetCurrentState().OnEnter();
    }

    
    

}
