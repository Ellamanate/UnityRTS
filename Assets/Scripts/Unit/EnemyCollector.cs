using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public abstract class EnemyCollector : MonoBehaviour
{
    public IReadOnlyCollection<IDamageable> DamageablesInRange { get => _damageablesInRange.AsReadOnly(); }
    public IReadOnlyCollection<Unit> EnemysInRange { get => _enemysInRange.AsReadOnly(); }

    [SerializeField] private List<IDamageable> _damageablesInRange = new List<IDamageable>();
    [SerializeField] private List<Unit> _enemysInRange = new List<Unit>();
    private IReadOnlyCollection<string> _enemyTags;

    protected virtual void WhenEnable() { }

    protected virtual void WhenDisable() { }

    protected virtual void WhenDamageableDestroy(IDamageable _destroyed) { }

    protected virtual void WhenDamageableEnter(IDamageable _damageable) { }

    protected virtual void WhenDamageableExit(IDamageable _damageable) { }

    private void OnEnable()
    {
        _enemyTags = _enemyTags = AllianceSystem.Instance.GetEnemyTags(gameObject.tag);
        Events.OnUnitDestroy.Subscribe(OnUnitDestroy);
        Events.OnChangeAlliance.Subscribe(SetEnemyTags);
        WhenEnable();
    }

    private void OnDisable()
    {
        Events.OnUnitDestroy.UnSubscribe(OnUnitDestroy);
        Events.OnChangeAlliance.UnSubscribe(SetEnemyTags);
        WhenDisable();
    }

    private void OnUnitDestroy(IDamageable _destroyed)
    {
        _damageablesInRange.Remove(_destroyed);
        _enemysInRange.Remove(_destroyed as Unit);
        WhenDamageableDestroy(_destroyed);
    }

    private void SetEnemyTags(IReadOnlyCollection<string> _newTags)
    {
        if (_newTags != null)
        {
            if (_enemyTags.Count == 0)
            {
                _enemyTags = _newTags;
            }
            else if (_newTags.Count != 0)
            {
                List<Unit> _newEnemys = new List<Unit>();

                foreach (Unit _enemy in _damageablesInRange)
                {
                    foreach (string _enemyTag in _newTags)
                    {
                        if (_enemy.gameObject.CompareTag(_enemyTag))
                            _newEnemys.Add(_enemy);
                    }
                }

                _enemyTags = _newTags;
                _enemysInRange = _newEnemys;
            }
            else
            {
                _enemyTags = _newTags;
                _enemysInRange.Clear();
            }
        }
    }

    private void OnTriggerEnter(Collider _collider)
    {
        if (TypeChecker<IDamageable>.CheckCollider(_collider, out IDamageable _damageable))
        {
            _damageablesInRange.Add(_damageable);

            if (IsEnemyCollider(_collider, out Unit _unit))
                _enemysInRange.Add(_unit);

            WhenDamageableEnter(_damageable);
        }
    }

    private void OnTriggerExit(Collider _collider)
    {
        if (TypeChecker<IDamageable>.CheckCollider(_collider, out IDamageable _damageable))
        {
            _damageablesInRange.Remove(_damageable);

            if (IsEnemyCollider(_collider, out Unit _unit))
                _enemysInRange.Remove(_unit);

            WhenDamageableExit(_damageable);
        }
    }

    private bool IsEnemyCollider (Collider _collider, out Unit _unit)
    {
        if (_collider.attachedRigidbody != null)
            _unit = _collider.attachedRigidbody.GetComponent<Unit>();
        else
            _unit = null;

        if (_unit != null)
        {
            string _collisionTag = _collider.tag;

            foreach (string _enemyTag in _enemyTags)
            {
                if (_enemyTag == _collisionTag)
                    return true;
            }
        }

        return false;
    }
}