using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseAction
{
    public enum ActionType
    {
        None = 0,
        Holster = 5,
        Draw = 4,
        Reload = 3,
        AimIn = 2,
        AimOut = 1
    }

    public ActionType actionType;
    public int Priority;
    public Action<BaseAction> OnExit;
    public bool running;

    public BaseAction( int priority,ActionType type)
    {
        Priority = priority;
        actionType = type;
    }

    public virtual void Enter()
    {
        running = true;
    }
    public virtual void Exit()
    {
        OnExit?.Invoke(this);
        running = false;
    }
    public virtual void Update()
    { 
    
    }
   
}
