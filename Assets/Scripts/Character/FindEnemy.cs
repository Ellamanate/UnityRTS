using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindEnemy : MonoBehaviour
{
    public List<Transform> Enemys = new List<Transform>();
    [HideInInspector] public List<string> EnemyTags = new List<string>();

    private void Start()
    {
        Events.OnDamageableDestroy.Subscribe(OnDamageableDestroy);
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform _parent = other.attachedRigidbody.gameObject.transform;
        string _collisionTag = _parent.tag;
        foreach (string _enemyTag in EnemyTags)
        {
            if (_enemyTag == _collisionTag)
            {
                if (_parent.GetComponent<Damageable>() != null)
                {
                    Enemys.Add(_parent);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Transform _parent = other.attachedRigidbody.gameObject.transform;
        string _collisionTag = _parent.tag;
        foreach (string _enemyTag in EnemyTags)
        {
            if (_enemyTag == _collisionTag)
            {
                if (_parent.GetComponent<Damageable>() != null)
                {
                    Enemys.Remove(_parent);
                }
            }
        }
    }

    public Transform FindNearest()
    {
        if (Enemys.Count == 1)
        {
            return Enemys[0];
        }
        Transform _potentialTarget = null;
        float _minDistance = Mathf.Infinity;
        foreach (Transform _enemy in Enemys)
        {
            float _distance = Vector3.Distance(transform.position, _enemy.position);
            if (_distance < _minDistance)
            {
                _minDistance = _distance;
                _potentialTarget = _enemy;
            }
        }
        return _potentialTarget;
    }

    private void OnDamageableDestroy(Damageable _destroyed)
    {
        Enemys.Remove(_destroyed.transform);
    }
}
