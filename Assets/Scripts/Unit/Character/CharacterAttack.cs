using System.Collections;
using UnityEngine;
using UnityEngine.Events;


public class CharacterAttack : EnemyCollector
{
    public UnityAction OnTargetDead;
    public UnityAction OnTargetCollision;

    [SerializeField] private NearestEnemyFinder _nearestEnemyFinder;
    [SerializeField] private DamageType _damageType;
    [SerializeField] private int _damage;
    [SerializeField] private bool _targetCollision = false;
    [SerializeField] private bool _isTargetSearching = true;
    private IDamageable _target;
    private IEnumerator _find;

    public DamageType DamageType => _damageType;
    public int Damage { get => _damage; set { if (value >= 0) _damage = value; } }
    public bool TargetCollision => _targetCollision;
    public IDamageable Target { get => _target; set { _target = value; CheckCollision(); } }

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

    public void AttackOrder(IDamageable newTarget, out bool collision)
    {
        if (Target != newTarget)
        {
            _isTargetSearching = false;
            Target = newTarget;
        }

        collision = _targetCollision;
    }

    protected override void WhenEnable()
    {
        StartCoroutine(_find);
    }

    protected override void WhenDisable()
    {
        StopAllCoroutines();
    }

    protected override void WhenDamageableDestroy(IDamageable destroyed)
    {
        if (this != null) // this == null при одновременной смерти
        {
            if (destroyed == Target)
            {
                Target = null;
                _isTargetSearching = true;
                OnTargetDead.Invoke();
            }
        }
    }

    protected override void WhenDamageableEnter(IDamageable damageable)
    {
        if (damageable == Target)
            CheckCollision();
    }

    protected override void WhenDamageableExit(IDamageable damageable)
    {
        if (damageable == Target)
            CheckCollision();
    }

    private void Awake()
    {
        _find = SetTarget(3.5f, 3.5f, 0.5f);
    }

    private void CheckCollision()
    {
        _targetCollision = false;

        foreach (IDamageable _enemy in _damageablesInRange)
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

    private IEnumerator SetTarget(float timer, float changeTargetDelay, float delay)
    {
        Target = _nearestEnemyFinder.FindNearest();
        IDamageable nearestTarget;

        yield return new WaitForSeconds(delay);

        while (true)
        {
            if (_isTargetSearching == true)
            {
                nearestTarget = _nearestEnemyFinder.FindNearest();

                if (Target != nearestTarget)
                {
                    if (Target == null)
                    {
                        Target = nearestTarget;
                    }
                    else if (timer >= changeTargetDelay)
                    {
                        timer = 0;

                        if (Random.Range(1, 5) >= 2)
                            Target = nearestTarget;
                    }
                }

                timer += delay;
            }

            yield return new WaitForSeconds(delay);
        }
    }
}
