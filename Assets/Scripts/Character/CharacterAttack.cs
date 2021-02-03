using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterAttack : MonoBehaviour
{
    public FindEnemy FindEnemy;
    public int Damage;
    public bool TargetCollision { get { return _targetCollision; } }
    public UnityEvent OnEnemyEnterRange { get { return _onEnemyEnterRange; } }
    public UnityEvent OnEnemyExitRange { get { return _onEnemyExitRange; } }
    public UnityEvent OnTergetDead { get { return _onTergetDead; } }
    public Damageable TargetDamagable { get { return _targetDamagable; } }

    private UnityEvent _onEnemyEnterRange = new UnityEvent();
    private UnityEvent _onEnemyExitRange = new UnityEvent();
    private UnityEvent _onTergetDead = new UnityEvent();
    public List<Transform> _enemysInAttackRange = new List<Transform>();
    public Damageable _targetDamagable;
    public Transform _target;
    public bool _targetCollision = false;
    private bool _targetChanged = false;
    private bool _findingTarget = true;
    private IEnumerator _find;
    private List<string> EnemyTags = new List<string>();

    public Transform Target
    {
        get { return _target; }
        set
        {
            if (value != null)
            {
                _target = value;
                _targetDamagable = value.GetComponent<Damageable>();
            }
            else
            {
                _target = null;
                _targetDamagable = null;
            }
        }
    }

    private void Start()
    {
        Events.OnDamageableDestroy.Subscribe(OnDamageableDestroy);
        EnemyTags = Static.AllianceSystem.GetEnemyTags(gameObject.tag);
        FindEnemy.EnemyTags = EnemyTags;
        _find = SetTarget(3.5f, 3.5f, 0.5f);
        StartCoroutine(_find);
    }

    public void SimpleAttack()
    {
        if (Target != null)
        {
            _targetDamagable.ApplyDamage(Damage);
        }
    }

    public void DropTarget()
    {
        if (_find != null)
        {
            StopCoroutine(_find);
        }
        _findingTarget = false;
        Target = null;
    }

    public void AllowSetTarget()
    {
        _findingTarget = true;
        StartCoroutine(_find);
    }

    private IEnumerator SetTarget(float _timer, float _changeTargetDelay, float _delay)
    {
        Target = FindEnemy.FindNearest();
        Transform _findedTarget;
        yield return new WaitForSeconds(_delay);
        while (true)
        {
            if (_findingTarget == true)
            {
                _findedTarget = FindEnemy.FindNearest();
                if (Target != _findedTarget)
                {
                    if (Target == null)
                    {
                        Target = _findedTarget;
                    }
                    else if (_timer >= _changeTargetDelay)
                    {
                        _timer = 0;
                        if (Random.Range(1, 5) >= 2)
                        {
                            Target = _findedTarget;
                        }
                    }
                    _targetChanged = true;
                }
                if (_targetChanged == true)
                {
                    CheckCollision();
                }
                _timer += _delay;
            }
            yield return new WaitForSeconds(_delay);
        }
    }

    public void CheckCollision()
    {
        _targetCollision = false;
        foreach (Transform _enemy in _enemysInAttackRange)
        {
            if (_enemy == Target)
            {
                _targetCollision = true;
                break;
            }
        }
        _targetChanged = false;
    }

    private void OnDamageableDestroy(Damageable _destroyed)
    {
        if (this != null) // this == null при одновременной смерти
        {
            _enemysInAttackRange.Remove(_destroyed.transform);
            if (_destroyed == _targetDamagable)
            {
                Target = null;
                _targetCollision = false;
                AllowSetTarget();
                _onTergetDead.Invoke();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform _parent = other.attachedRigidbody.gameObject.transform;
        string _collisionTag = _parent.tag;
        foreach (string _enemyTag in EnemyTags)
        {
            if (_enemyTag == _collisionTag)
            {
                _enemysInAttackRange.Add(_parent);
            }
        }
        if (_parent == Target)
        {
            _onEnemyEnterRange.Invoke();
            _targetCollision = true;
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
                _enemysInAttackRange.Remove(_parent);
            }
        }
        if (_parent == Target)
        {
            _onEnemyExitRange.Invoke();
            _targetCollision = false;
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
