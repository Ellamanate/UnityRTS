using System.Collections;
using UnityEngine;
using System.Linq;


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

    public void Attack(IDamageable _target)
    {
        _characterAttack.AttackOrder(_target, out bool _collision);

        if (!_collision)
            MoveToDamageable(_target);
    }

    public void MoveToDamageable(IDamageable _damageable)
    {
        if (_damageable == GetComponent<IDamageable>())
            _characterMove.Block = true;
        else if (!_characterAttack.DamageablesInRange.Contains(_damageable) | _characterMove.IsMove)
            MoveToPoint(GetOffset(_damageable.GameObject.transform.position));
    }

    public void MoveOrder(Vector3 _point)
    {
        _characterAttack.DropTarget();
        MoveToPoint(_point);
    }

    public void AttackMove(Vector3 _point)
    {
        if (_nextPoint != _point)
            _nextPoint = _point;

        if (_goToNextPoint != true)
            _goToNextPoint = true;

        if (!_characterAttack.TargetCollision)
        {
            _characterAttack.AllowSetTarget();
            _isMoveOrder = false;
            MoveState();
            _characterMove.MoveToPoint(_point);
        }
    }

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

    public void AddDamage(int _damage)
    {
        _characterAttack.Damage += _damage;
    }

    public void RemoveDamage(int _damage)
    {
        _characterAttack.Damage -= _damage;
    }

    private void Awake()
    {
        _chase = ChaseEnemy(0.25f);
    }

    private void Update()
    {
        if ((_characterAttack.TargetCollision || _characterAnimator.AttackAnimation) && !_isMoveOrder && _characterAttack.Target != null)
        {
            Vector3 _difference = (_characterAttack.Target.GameObject.transform.position - transform.position).normalized;
            _difference.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_difference), Time.deltaTime * _rotationSpeedWhenAttack);
        }
    }

    private void MoveToPoint(Vector3 _point)
    {
        _isMoveOrder = true;
        MoveState();
        _characterMove.MoveToPoint(_point);
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

    private IEnumerator ChaseEnemy(float _delay)
    {
        while (true)
        {
            if (!_isMoveOrder & _characterAttack.Target != null & !_characterAttack.TargetCollision & !_characterMove.Block)
            {
                if (!_characterAnimator.MovingAnimation)
                    _characterAnimator.StartMove();

                _characterMove.MoveToPoint(GetOffset(_characterAttack.Target.GameObject.transform.position));
            }

            yield return new WaitForSeconds(_delay);
        }
    }

    private Vector3 GetOffset(Vector3 _target)
    {
        return _target - (_target - transform.position).normalized * _characterMove.Speed * Time.deltaTime;
    }
}
