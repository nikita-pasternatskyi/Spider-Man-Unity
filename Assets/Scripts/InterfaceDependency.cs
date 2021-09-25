using UnityEngine;

[System.Serializable]
public class InterfaceDependency<T> where T : class
{
    [SerializeField] private MonoBehaviour _monoBehaviour;

    public T GetValue()
    {
        if (_monoBehaviour is T)
            return _monoBehaviour as T;

        Debug.LogError(_monoBehaviour.name + " needs to implement " + nameof(T));
        _monoBehaviour = null;
        return default(T);
    }

    public void CheckValue()
    {
        if (_monoBehaviour is T)
            return;

        Debug.LogError(_monoBehaviour.name + " needs to implement " + nameof(T));
        _monoBehaviour = null;
    }

}