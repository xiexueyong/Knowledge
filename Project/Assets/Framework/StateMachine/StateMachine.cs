using System;
using System.Collections.Generic;
using UnityEngine;


    public interface IState<T> where T : struct
    {
        T StateType { get; }
        bool CanEnter();
        void Enter();
        void Exit();
        void Update();
    }

    public class StateRelation<T> where T : struct
    {
        public T FromState;

        public T ToState;
    }

public abstract class StateMachine<T> where T : struct
{
    protected Dictionary<T, IState<T>> _stateDicts = new Dictionary<T, IState<T>>();

    protected Dictionary<T, List<IState<T>>> _toStateDicts = new Dictionary<T, List<IState<T>>>();

    protected IState<T> _currentState;

    private StateRelation<T>[] _relations;

    private T _startStateType;

    public StateMachine(StateRelation<T>[] relations, T startStateType)
    {
        _relations = relations;
        _startStateType = startStateType;

        for (var i = 0; i < relations.Length; ++i)
        {
            if (!_stateDicts.ContainsKey(relations[i].FromState))
            {
                _stateDicts.Add(relations[i].FromState, NewState(relations[i].FromState));
            }

            if (!_stateDicts.ContainsKey(relations[i].ToState))
            {
                _stateDicts.Add(relations[i].ToState, NewState(relations[i].ToState));
            }

            if (!_toStateDicts.ContainsKey(relations[i].FromState))
            {
                _toStateDicts.Add(relations[i].FromState, new List<IState<T>>());
            }

            if (_stateDicts[relations[i].ToState] != null)
            {
                _toStateDicts[relations[i].FromState].Add(_stateDicts[relations[i].ToState]);
            }
        }
    }

    protected abstract IState<T> NewState(T type);

    public virtual void Start()
    {
        _currentState = _stateDicts[_startStateType];

        //LogUtil.Log(GetType().Name + " Enter " + _startStateType);

        _currentState.Enter();
    }
    public virtual void Exit()
    {
      
    }

    public T GetCurrentStateType()
    {
        if (this._currentState != null)
        {
            return this._currentState.StateType;
        }
        else
        {
            throw new Exception("Current State Is Null");
        }
    }

    private void Enter(IState<T> state)
    {
        //Debug.Log(GetType().Name + " Leave " + _currentState.StateType);
        _currentState.Exit();

        _currentState = state;

        // Debug.Log(Time.realtimeSinceStartup + " " + GetType().Name + " Enter " + _currentState.StateType);
        _currentState.Enter();
    }

    public void Enter(T stateType)
    {
        Enter(_stateDicts[stateType]);
    }

    public void EnterNextState()
    {
        var curStateType = _currentState.StateType;
        for (var i = 0; i < _relations.Length; ++i)
        {
            var stateType = _relations[i].FromState;
            if (!stateType.Equals(curStateType))
            {
                continue;
            }

            var toStates = _toStateDicts[stateType];
            for (var j = 0; j < toStates.Count; ++j)
            {
                if (toStates[j].CanEnter())
                {
                    Enter(toStates[j]);
                    return;
                }
            }
        }
        _currentState = null;

        //TODO: Enter ERROR State

    }

    public void Update()
    {
        _currentState?.Update();
    }
}
