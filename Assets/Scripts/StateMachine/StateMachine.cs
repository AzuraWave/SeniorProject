
using System;
using System.Collections.Generic;
using UnityEngine;
public class StateMachine
{
    public State CurrentState { get; private set; }
    private readonly List<Func<State>> globalTransitionConditions = new();
    public void AddGlobalTransition(Func<State> condition)
    {
        globalTransitionConditions.Add(condition);
    }

    private Dictionary<Type, State> states = new Dictionary<Type, State>();
    
    public void AddState(State state)
    {
        if (!states.ContainsKey(state.GetType()))
        {
            states.Add(state.GetType(), state);
        }
        else
        {
            Debug.LogWarning($"State {state.GetType().Name} already exists in the state machine.");
        }
    }

    public T GetState<T>() where T : State
    {
        return (T)states[typeof(T)];
    }

    public void Initialize(State startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void TransitionTo(State nextState, bool forced = false)
    {
            
            if (CurrentState != null)
            {
                CurrentState.Exit();
            }
            CurrentState = nextState;
            CurrentState?.Enter();
        
    }

    public void Execute()
    {

        foreach (var condition in globalTransitionConditions)
        {
            State targetState = condition.Invoke();
            if (targetState != null)
            {
                TransitionTo(targetState);
                return;
            }
        }
        var nextState = CurrentState.CheckTransitions();
        if (nextState != null)
        {
            TransitionTo(nextState);
        }
        CurrentState.Execute();
    }
    public void FixedExecute()
    {
        CurrentState?.FixedExecute();
    }
    
}
