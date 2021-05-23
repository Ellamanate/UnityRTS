using System.Collections.Generic;
using UnityEngine;


public class ObjectPool<T> where T : MonoBehaviour
{
    public Transform Pool { get => _pool; }
    public T Prefab { get => _prefab; }

    private T _prefab;
    private Transform _pool;
    private List<T> _objectsPool = new List<T>();

    public ObjectPool(T prefab, Transform pool)
    {
        _prefab = prefab;
        _pool = pool;
    }

    public T[] GetObjects(int count)
    {
        if (_objectsPool.Count < count)
            CreateObjects(count - _objectsPool.Count);

        T[] _objects = new T[count];

        for (int i = 0; i < _objectsPool.Count; i++)
        {
            if (i < _objects.Length)
                _objects[i] = _objectsPool[i];
            else
                _objectsPool[i].transform.SetParent(Pool);
        }

        return _objects;
    }

    public void BackToPool(T _target)
    {
        if (_objectsPool.Contains(_target))
            _target.transform.SetParent(Pool);
    }

    public void BackToPoolAll()
    {
        for (int i = 0; i < _objectsPool.Count; i++)
            _objectsPool[i].transform.SetParent(Pool);
    }

    private void CreateObjects(int count)
    {
        for (int i = 0; i < count; i++)
        {
            T newObject = GameObject.Instantiate(_prefab, _pool);
            _objectsPool.Add(newObject);
        }
    }
}