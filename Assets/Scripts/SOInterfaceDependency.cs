using UnityEngine;

[System.Serializable]
public class SOInterfaceDependency<T> where T : class
{
    [SerializeField] private ScriptableObject _scriptableObject;

    public T GetValue()
    {
        if (_scriptableObject == null)
            throw new MissingReferenceException(nameof(_scriptableObject));

        if (_scriptableObject is T)
            return _scriptableObject as T;

        Debug.LogError($"{_scriptableObject.name} needs to implement + {nameof(T)}");
        _scriptableObject = null;
        return default(T);
    }

    public void CheckValue()
    {
        if (_scriptableObject == null)
            return;
        if (_scriptableObject is T)
            return;

        Debug.LogError($"{_scriptableObject.name} needs to implement + {nameof(T)}");
        _scriptableObject = null;
    }

}
