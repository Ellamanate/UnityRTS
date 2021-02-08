using System.Collections.Generic;
using UnityEngine;


public class ObjectPool
{
    public Transform Pool { get => _pool; }
    public GameObject Prefab { get => _prefab; }
    public IReadOnlyCollection<Transform> Objects { get => _objectsPool.AsReadOnly(); }

    private Transform _pool;
    private GameObject _prefab;
    private List<Transform> _objectsPool = new List<Transform>();

    public ObjectPool(GameObject _initPrefab, Transform _initPool)
    {
        _prefab = _initPrefab;
        _pool = _initPool;
    }

    public Transform[] GetObjects(int _count)
    {
        if (_objectsPool.Count < _count)
            CreateObjects(_count - _objectsPool.Count);

        Transform[] _objects = new Transform[_count];

        for (int i = 0; i < _objectsPool.Count; i++)
        {
            if (i < _objects.Length)
                _objects[i] = _objectsPool[i];
            else
                _objects[i].SetParent(Pool);
        }

        return _objects;
    }

    public void BackToPool(Transform _target)
    {
        if (_objectsPool.Contains(_target))
            _target.SetParent(Pool);
    }

    public void BackToPoolAll()
    {
        for (int i = 0; i < _objectsPool.Count; i++)
            _objectsPool[i].SetParent(Pool);
    }

    private void CreateObjects(int _count)
    {
        for (int i = 0; i < _count; i++)
        {
            GameObject _newObject = GameObject.Instantiate(Prefab, Pool);
            _objectsPool.Add(_newObject.transform);
        }
    }
}