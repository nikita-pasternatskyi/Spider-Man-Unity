using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public abstract class SimpleStateMachine<TStateType> : MonoBehaviour where TStateType : IState
{
    protected TStateType CurrentState { get; private set; }
    private Dictionary<Type, TStateType> _states;
    private Dictionary<Type, object> _dependencyResolvers;

    protected void InitStates()
    {
        _states = new Dictionary<Type, TStateType>();
        foreach (var item in GetStates())
        {
            _states.Add(item.GetType(), item);
        }
    }
    protected void InitDependencies()
    {
        _dependencyResolvers = new Dictionary<Type, object>();
        foreach (var item in CreateStateDependencyResolvers())
        {
            if (item == null)
            {
                throw new MissingReferenceException(nameof(item));
            }

            _dependencyResolvers.Add(item.GetType(), item);
        }
        ResolveStateDependencies();
    }
    protected void ChangeState(TStateType newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
    private IEnumerable<TStateType> GetStates()
    {
        Type type = GetType();

        foreach (var field in type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
        {
            if (field.GetCustomAttributes<StateAttribute>().Count() == 0)
                continue;

            yield return (TStateType)field.GetValue(this);
        }
    }
    private IEnumerable<object> CreateStateDependencyResolvers()
    {
        Type type = GetType();

        foreach (var field in type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
        {
            if (field.GetCustomAttributes<StateDependencyResolverAttribute>().Count() == 0)
                continue;

            yield return field.GetValue(this);
        }
    }
    private void ResolveStateDependencies()
    {
        foreach (var state in GetStates())
        {
            Type type = state.GetType();

            foreach (var fieldInfo in type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy))
            {
                if (fieldInfo.GetCustomAttributes<StateInjectAttribute>().Count() == 0)
                    continue;
                _dependencyResolvers.TryGetValue(fieldInfo.FieldType, out object resolver);

                print("resolve");
                fieldInfo.SetValue(state, resolver);
            }
        }
    }

}

public interface IState
{
    public void Enter();
    public void Exit();
}

