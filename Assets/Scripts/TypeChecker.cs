using System.Collections;
using UnityEngine;


public static class TypeChecker<T>
{
    public static bool CheckSelectable(ISelectable _selectable, out T _type)
    {
        try { _type = (T)_selectable; }
        catch { _type = default; }

        return _type != null;
    }

    public static bool CheckGameObject(GameObject _object, out T _type)
    {
        _type = _object.GetComponent<T>();
        return _type != null;
    }

    public static bool CheckCollider(Collider _collider, out T _type)
    {
        if (_collider.attachedRigidbody != null)
            _type = _collider.attachedRigidbody.GetComponent<T>();
        else
            _type = default;

        return _type != null;
    }
}