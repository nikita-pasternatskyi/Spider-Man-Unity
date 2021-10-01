using System;
using System.Collections.Generic;

public class StateTransitions<T> where T : State
{
    private readonly HashSet<StateTransition<T>> _stateTransitionHashSet;
    public StateTransitions()
    {
        _stateTransitionHashSet = new HashSet<StateTransition<T>>();
    }

    public void AddTransition(T to, Func<bool> condition)
    {
        _stateTransitionHashSet.Add(new StateTransition<T>(to, condition));
    }

    public bool CheckConditions()
    {
        foreach (var transition in _stateTransitionHashSet)
        {
            if (transition.Condition() == true)
                return (true);
        }
        return false;
    }

    public T GetToState()
    {
        foreach (var transition in _stateTransitionHashSet)
        {
            if (transition.Condition() == true)
                return transition.To;
        }
        return default(T);
    }
}

public class StateTransition<T> where T : State
{
    public T To { get; private set; }
    public Func<bool> Condition { get; private set; }


    public StateTransition(T to, Func<bool> condition)
    {
        Condition = condition;
        To = to;
    }
}
