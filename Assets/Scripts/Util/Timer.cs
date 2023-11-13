using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{

    private float timer;
    private Action action;
    private bool isDone;

    // Update is called once per frame
    void Update()
    {
        OnTick();
        RecycleTimer();
    }

    void OnTick()
    {
        if (!gameObject.activeSelf)
            return;
        if(timer > 0 && !isDone)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                action.Invoke();
                isDone = true;
            }
        }
    }

    void RecycleTimer()
    {
        if(isDone)
        {
            action = null;
            GameObjectPool.Instance.RecycleObject(gameObject);
        }
    }

    public void CreatTimer(float timer,Action action,bool isDone = false)
    {
        this.timer = timer;
        this.action = action;
        this.isDone = isDone;
    }
}
