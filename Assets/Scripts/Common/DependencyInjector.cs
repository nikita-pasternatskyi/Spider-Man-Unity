using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public abstract class DependencyInjector
{
    private Dictionary<Type, object> _dependencyResolvers;

    protected void InjectDependencies()
    {
        _dependencyResolvers = new Dictionary<Type, object>();
        foreach (var item in CreateDependencyResolvers())
        {
            if (item == null)
            {
                throw new MissingReferenceException(nameof(item));
            }

            _dependencyResolvers.Add(item.GetType(), item);
        }
        ResolveDependencies();
    }

    private IEnumerable<object> GetDependencies()
    {
        Type type = GetType();

        foreach (var field in type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
        {
            yield return field.GetValue(this);
        }
    }

    private IEnumerable<object> CreateDependencyResolvers()
    {
        Type type = GetType();

        foreach (var field in type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
        {
            if (field.GetCustomAttributes<DependencyResolverAttribute>().Count() == 0)
                continue;

            yield return field.GetValue(this);
        }
    }

    private void ResolveDependencies()
    {
        foreach (var dependencyNeeder in GetDependencies())
        {
            Type type = dependencyNeeder.GetType();

            foreach (var fieldInfo in type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy))
            {
                if (fieldInfo.GetCustomAttributes<InjectAttribute>().Count() == 0)
                    continue;
                _dependencyResolvers.TryGetValue(fieldInfo.FieldType, out object resolver);

                fieldInfo.SetValue(dependencyNeeder, resolver);
            }
        }
    }

}

[AttributeUsage(AttributeTargets.Field)]
public class InjectAttribute : Attribute
{

}

[AttributeUsage(AttributeTargets.Field)]
public class DependencyResolverAttribute : Attribute
{

}
