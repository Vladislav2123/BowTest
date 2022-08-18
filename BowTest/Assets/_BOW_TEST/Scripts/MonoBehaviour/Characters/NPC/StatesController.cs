using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class StatesController : MonoBehaviour
{
    protected Dictionary<Type, IState> _states;
    private IState _currentState;

    protected virtual void Awake()
    {
        _states = new Dictionary<Type, IState>();
        InitializeStates();
    }

    private void Update()
    {
        if (_currentState != null) _currentState.Update();
    }

    protected abstract void InitializeStates();

    protected void SetState(IState newState)
    {
        if (_currentState != null) _currentState.Exit();

        _currentState = newState;
        _currentState.Enter();
    }

    public void ResetState()
    {
        if (_currentState != null) _currentState.Exit();
        _currentState = null;
    }

    protected IState GetState<T>() where T : IState
    {
        var type = typeof(T);
        return _states[type];
    }
}