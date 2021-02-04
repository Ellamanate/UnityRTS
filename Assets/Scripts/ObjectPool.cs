using System.Collections.Generic;
using UnityEngine;


public class ObjectPool
{
    public Transform Pool;
    public GameObject Prefab;
    public List<Transform> Objects = new List<Transform>();

    public ObjectPool(GameObject _prefab, Transform _pool)
    {
        Prefab = _prefab;
        Pool = _pool;
    }

    public Transform[] GetObjects(int _count)
    {
        if (Objects.Count < _count)
            CreateObjects(_count - Objects.Count);

        Transform[] _objects = new Transform[_count];
        for (int i = 0; i < Objects.Count; i++)
        {
            if (i < _objects.Length)
                _objects[i] = Objects[i];
            else
                Objects[i].SetParent(Pool);
        }

        return _objects;
    }

    public void BackToPool(Transform _target)
    {
        if (Objects.Contains(_target))
            _target.SetParent(Pool);
    }

    public void BackToPoolAll()
    {
        for (int i = 0; i < Objects.Count; i++)
            Objects[i].SetParent(Pool);
    }

    private void CreateObjects(int _count)
    {
        for (int i = 0; i < _count; i++)
        {
            GameObject _newObject = GameObject.Instantiate(Prefab, Pool);
            Objects.Add(_newObject.transform);
        }
    }
}