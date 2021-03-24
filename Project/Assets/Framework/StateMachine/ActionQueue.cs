using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionQueue
{

    public List<BaseAction> actionsQueue;





    public virtual void Update()
    {
        if (actionsQueue.Count > 0 && actionsQueue[0] != null)
        {
            if (!actionsQueue[0].running)
            {
                actionsQueue[0].Enter();
            }
        }

        if (actionsQueue.Count > 0 && actionsQueue[0] != null)
        {
            if (actionsQueue[0].running)
            {
                actionsQueue[0].Update();
            }
        }
    }
    public bool isRunning()
    {
        return actionsQueue.Count > 0 && actionsQueue[0] != null && actionsQueue[0].running;
    }

    protected void OnActionExit(BaseAction action)
    {
        if (actionsQueue.Contains(action))
        {
            actionsQueue.Remove(action);
        }
    }

    protected bool EnQueue(BaseAction _action)
    {
        if (actionsQueue.Count <= 0)
        {
            actionsQueue.Add(_action);
            _action.OnExit = OnActionExit;
            return true;
        }

        //确认队列内是否已经包含该动作
        for (int i = actionsQueue.Count - 1; i >= 0; i--)
        {
            BaseAction act = actionsQueue[i];
            if (act != null && act.actionType == _action.actionType)
            {
                return false;
            }
        }

        //删除优先级较小的
        for (int i = actionsQueue.Count - 1; i >= 0; i--)
        {
            BaseAction act = actionsQueue[i];
            if (act == null || act.Priority <= _action.Priority)
            {
                actionsQueue.RemoveAt(i);
                if (act != null && act.running)
                {
                    act.OnExit = null;
                    act.Exit();
                }
            }
        }

        actionsQueue.Add(_action);
        _action.OnExit = OnActionExit;
        return true;
    }

}
