using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public abstract class StateMachine<TStateType> : MonoBehaviour where TStateType : State
{
    private TStateType _currentState;
    private Dictionary<Type, TStateType> _states;
    private Dictionary<Type, object> _dependencyResolvers;

    private void Awake()
    {
        StateMachineAwake();
        _states = new Dictionary<Type, TStateType>();
        foreach (var item in GetStates())
        {
            _states.Add(item.GetType(), item);
            Debug.Log(item);
        }
    }
    private void Start()
    {
        _dependencyResolvers = new Dictionary<Type, object>();
        foreach (var item in CreateStateDependencyResolvers())
        {
            if (item == null)
            {
                throw new MissingReferenceException(nameof(item));
            }

            print(item);
            print(item.GetType());
            print(item.ToString());
            _dependencyResolvers.Add(item.GetType(), item);
        }
        ResolveStateDependencies();
        ChangeState(StartState());
    }
    private void Update()
    {
        _currentState.LogicUpdate();
        StateMachineUpdate();
    }
    private void FixedUpdate()
    {
        _currentState.FixedUpdate();
        StateMachineFixedUpdate();
    }
    private void OnEnable() => StateMachineEnable();
    private void OnDisable() => StateMachineDisable();

    private void OnCollisionEnter2D(Collision2D collision) => _currentState.OnCollisionEnter2D(collision);

    private void OnTriggerEnter2D(Collider2D collider) => _currentState.OnTriggerEnter2D(collider);

    private void OnTriggerExit2D(Collider2D collision) => _currentState.OnTriggerExit2D(collision);

    private void OnCollisionExit2D(Collision2D collision) => _currentState.OnCollisionExit2D(collision);

    protected void ChangeState(TStateType stateEnum)
    {
        _states.TryGetValue(stateEnum.GetType(), out TStateType newState);
        if (newState == null)
            throw new System.Exception($"{stateEnum} is null! It doesnt exist in the _playerStates Dictionary!");
        _currentState?.Exit();
        _currentState = stateEnum;
        _currentState = newState;
        _currentState.Enter();
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

            foreach (var fieldInfo in type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (fieldInfo.GetCustomAttributes<StateDependencyAttribute>().Count() == 0)
                    continue;
                _dependencyResolvers.TryGetValue(fieldInfo.FieldType, out object resolver);
                fieldInfo.SetValue(state, resolver);
            }
        }
    }

    protected virtual void StateMachineAwake()
    { }
    protected virtual void StateMachineUpdate()
    { }
    protected virtual void StateMachineFixedUpdate()
    { }

    protected virtual void StateMachineEnable()
    { }

    protected virtual void StateMachineDisable()
    { }

    protected abstract TStateType StartState();


}

[AttributeUsage(AttributeTargets.Field)]
public class StateAttribute : Attribute
{ 

}

[AttributeUsage(AttributeTargets.Field)]
public class StateDependencyAttribute : Attribute
{

}

[AttributeUsage(AttributeTargets.Field)]
public class StateDependencyResolverAttribute : Attribute
{

}

