using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CharacterAttack : EnemyCollector
{
    public DamageType DamageType { get => _damageType; set => _damageType = value; }
    public int Damage { get => _damage; set { if (value >= 0) _damage = value; } }
    public bool TargetCollision { get => _targetCollision; }
    public IDamageable Target { get => _target; set { _target = value; CheckCollision(); } }

    public UnityAction OnTargetDead;
    public UnityAction OnTargetCollision;

    [SerializeField] private NearestEnemyFinder _nearestEnemyFinder;
    [SerializeField] private DamageType _damageType;
    [SerializeField] private int _damage;
    private IDamageable _target;
    private IEnumerator _find;
    [SerializeField] private bool _targetCollision = false;
    [SerializeField] private bool _isTargetSearching = true;

    public void SimpleAttack()
    {
        if (Target != null)
            Target.ApplyDamage(DamageType, Damage);
    }

    public void DropTarget()
    {
        _isTargetSearching = false;
        Target = null;
    }

    public void AllowSetTarget()
    {
        if (Target == null)
            _isTargetSearching = true;
    }

    public void AttackOrder(IDamageable _newTarget, out bool _collision)
    {
        if (Target != _newTarget)
        {
            _isTargetSearching = false;
            Target = _newTarget;
        }

        _collision = _targetCollision;
    }

    protected override void WhenEnable()
    {
        StartCoroutine(_find);
    }

    protected override void WhenDisable()
    {
        StopAllCoroutines();
    }

    protected override void WhenDamageableDestroy(IDamageable _destroyed)
    {
        if (this != null) // this == null при одновременной смерти
        {
            if (_destroyed == Target)
            {
                Target = null;
                _isTargetSearching = true;
                OnTargetDead.Invoke();
            }
        }
    }

    protected override void WhenDamageableEnter(IDamageable _damageable)
    {
        if (_damageable == Target)
            CheckCollision();
    }

    protected override void WhenDamageableExit(IDamageable _damageable)
    {
        if (_damageable == Target)
            CheckCollision();
    }

    private void Awake()
    {
        _find = SetTarget(3.5f, 3.5f, 0.5f);
    }

    private void CheckCollision()
    {
        _targetCollision = false;

        foreach (IDamageable _enemy in DamageablesInRange)
        {
            if (_enemy == Target)
            {
                _targetCollision = true;
                break;
            }
        }

        if (OnTargetCollision != null & _targetCollision)
            OnTargetCollision.Invoke();
    }

    private IEnumerator SetTarget(float _timer, float _changeTargetDelay, float _delay)
    {
        Target = _nearestEnemyFinder.FindNearest();
        IDamageable _nearestTarget;

        yield return new WaitForSeconds(_delay);

        while (true)
        {
            if (_isTargetSearching == true)
            {
                _nearestTarget = _nearestEnemyFinder.FindNearest();

                if (Target != _nearestTarget)
                {
                    if (Target == null)
                    {
                        Target = _nearestTarget;
                    }
                    else if (_timer >= _changeTargetDelay)
                    {
                        _timer = 0;

                        if (Random.Range(1, 5) >= 2)
                            Target = _nearestTarget;
                    }
                }

                _timer += _delay;
            }

            yield return new WaitForSeconds(_delay);
        }
    }
}
