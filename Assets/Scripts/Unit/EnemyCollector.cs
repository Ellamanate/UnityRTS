using System.Collections.Generic;
using UnityEngine;


public abstract class EnemyCollector : MonoBehaviour
{
    [SerializeField] protected List<IDamageable> _damageablesInRange = new List<IDamageable>();
    [SerializeField] protected List<Unit> _enemysInRange = new List<Unit>();
    private IReadOnlyCollection<string> _enemyTags;

    public bool IsDamageableInRange(IDamageable damageable) => _damageablesInRange.Contains(damageable);

    protected virtual void WhenEnable() { }

    protected virtual void WhenDisable() { }

    protected virtual void WhenDamageableDestroy(IDamageable destroyed) { }

    protected virtual void WhenDamageableEnter(IDamageable damageable) { }

    protected virtual void WhenDamageableExit(IDamageable damageable) { }

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

    private void SetEnemyTags(IReadOnlyCollection<string> newTags)
    {
        if (newTags != null)
        {
            if (_enemyTags.Count == 0)
            {
                _enemyTags = newTags;
            }
            else if (newTags.Count != 0)
            {
                List<Unit> _newEnemys = new List<Unit>();

                foreach (Unit _enemy in _damageablesInRange)
                {
                    foreach (string _enemyTag in newTags)
                    {
                        if (_enemy.gameObject.CompareTag(_enemyTag))
                            _newEnemys.Add(_enemy);
                    }
                }

                _enemyTags = newTags;
                _enemysInRange = _newEnemys;
            }
            else
            {
                _enemyTags = newTags;
                _enemysInRange.Clear();
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (TypeChecker<IDamageable>.CheckCollider(collider, out IDamageable _damageable))
        {
            _damageablesInRange.Add(_damageable);

            if (IsEnemyCollider(collider, out Unit _unit))
                _enemysInRange.Add(_unit);

            WhenDamageableEnter(_damageable);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (TypeChecker<IDamageable>.CheckCollider(collider, out IDamageable _damageable))
        {
            _damageablesInRange.Remove(_damageable);

            if (IsEnemyCollider(collider, out Unit _unit))
                _enemysInRange.Remove(_unit);

            WhenDamageableExit(_damageable);
        }
    }

    private bool IsEnemyCollider (Collider collider, out Unit unit)
    {
        if (collider.attachedRigidbody != null)
            unit = collider.attachedRigidbody.GetComponent<Unit>();
        else
            unit = null;

        if (unit != null)
        {
            string _collisionTag = collider.tag;

            foreach (string _enemyTag in _enemyTags)
            {
                if (_enemyTag == _collisionTag)
                    return true;
            }
        }

        return false;
    }
}