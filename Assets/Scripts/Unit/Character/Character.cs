using System.Collections;
using System.Linq;
using UnityEngine;


public class Character : Unit
{
    [SerializeField] private CharacterAnimator _characterAnimator;
    [SerializeField] private CharacterAttack _characterAttack;
    [SerializeField] private CharacterMove _characterMove;
    [SerializeField] private float _rotationSpeedWhenAttack = 4;
    [SerializeField] private float _dropAntimationTime = 1;
    [SerializeField] private bool _isMoveOrder = false;
    [SerializeField] private bool _goToNextPoint = false;
    [SerializeField] private Vector3 _nextPoint;
    private IEnumerator _chase;
    private const float _chaseTimer = 0.25f;

    public override void Move(IDamageable damageable)
    {
        if (AllianceSystem.Instance.GetEnemyTags(tag).Contains(damageable.GameObject.tag))
            Attack(damageable);
        else
            MoveToDamageable(damageable);
    }

    public override void Move(Vector3 point) => MoveOrder(point);
    public override void Attack(IDamageable damageable) => AttackTarget(damageable);
    public override void Attack(Vector3 point) => AttackMove(point);
    public override void Collect(ItemPrefab item) => item.Collect(this);

    public override void Stop() 
    {
        _characterAttack.DropTarget();
        _characterMove.ResetPath();
        PathComplete();
    }

    public override void HoldPosition() => MoveToDamageable(this);

    public void AttackAnimationEnd()
    {
        _characterAttack.SimpleAttack();

        if (!_characterAttack.TargetCollision)
        {
            _characterMove.Block = false;
            _characterAnimator.StopAttack();
        }
    }

    public override void Destroy()
    {
        _characterMove.Block = true;
        _characterAttack.DropTarget();
        _characterAnimator.StartDeath();
        Destroy(this);
    }

    public override void WhenEnable()
    {
        base.WhenEnable();
        _characterAttack.OnTargetDead += OnTargetDead;
        _characterAttack.OnTargetCollision += OnTargetCollision;
        _characterMove.PathComplete += PathComplete;
        StartCoroutine(_chase);
    }

    public override void WhenDisable()
    {
        base.WhenDisable();
        _characterAttack.OnTargetDead -= OnTargetDead;
        _characterAttack.OnTargetCollision -= OnTargetCollision;
        _characterMove.PathComplete -= PathComplete;
        StopAllCoroutines();
    }

    public void AddDamage(int damage)
    {
        _characterAttack.Damage += damage;
    }

    public void RemoveDamage(int damage)
    {
        _characterAttack.Damage -= damage;
    }

    private void Awake()
    {
        _chase = ChaseEnemy(_chaseTimer);
    }

    private void Update()
    {
        if ((_characterAttack.TargetCollision || _characterAnimator.AttackAnimation) && !_isMoveOrder && _characterAttack.Target != null)
        {
            Vector3 difference = (_characterAttack.Target.GameObject.transform.position - transform.position).normalized;
            difference.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(difference), Time.deltaTime * _rotationSpeedWhenAttack);
        }
    }

    private void AttackTarget(IDamageable target)
    {
        _characterAttack.AttackOrder(target, out bool collision);

        if (!collision)
            MoveToDamageable(target);
    }

    private void MoveToDamageable(IDamageable target)
    {
        if (target == GetComponent<IDamageable>())
            _characterMove.Block = true;
        else if (!_characterAttack.IsDamageableInRange(target) | _characterMove.IsMove)
            MoveToPoint(GetOffset(target.GameObject.transform.position));
    }

    private void MoveOrder(Vector3 point)
    {
        _characterAttack.DropTarget();
        MoveToPoint(point);
    }

    private void AttackMove(Vector3 point)
    {
        if (_nextPoint != point)
            _nextPoint = point;

        if (_goToNextPoint != true)
            _goToNextPoint = true;

        if (!_characterAttack.TargetCollision)
        {
            _characterAttack.AllowSetTarget();
            _isMoveOrder = false;
            MoveState();
            _characterMove.MoveToPoint(point);
        }
    }

    private void MoveToPoint(Vector3 point)
    {
        _isMoveOrder = true;
        MoveState();
        _characterMove.MoveToPoint(point);
    }

    private void MoveState()
    {
        _characterMove.Block = false;
        _characterAnimator.StopAttack();
        _characterAnimator.StartMove();
    }

    private void OnTargetDead()
    {
        _characterMove.Block = false;
        _characterAnimator.StopAttack();

        if (_goToNextPoint)
            AttackMove(_nextPoint);
    }

    private void OnTargetCollision()
    {
        if (_characterAnimator.AttackAnimation) 
            _characterAnimator.ToDefaultState(_dropAntimationTime);

        _characterAnimator.StopMove();
        _characterAnimator.StartAttack();
        _characterMove.Block = true;
    }

    private void PathComplete()
    {
        if (_characterAttack.Target == null)
            _goToNextPoint = false;

        _isMoveOrder = false;
        _characterAnimator.StopMove();
        _characterAttack.AllowSetTarget();
    }

    private IEnumerator ChaseEnemy(float delay)
    {
        while (true)
        {
            if (!_isMoveOrder & _characterAttack.Target != null & !_characterAttack.TargetCollision & !_characterMove.Block)
            {
                if (!_characterAnimator.MovingAnimation)
                    _characterAnimator.StartMove();

                _characterMove.MoveToPoint(GetOffset(_characterAttack.Target.GameObject.transform.position));
            }

            yield return new WaitForSeconds(delay);
        }
    }

    private Vector3 GetOffset(Vector3 target) => target - (target - transform.position).normalized * _characterMove.Speed * Time.deltaTime;
}
