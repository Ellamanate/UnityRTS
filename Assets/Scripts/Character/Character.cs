using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Damageable
{
    public CharacterAnimator CharacterAnimator;
    public CharacterAttack CharacterAttack;
    public CharacterMove CharacterMove;
    public Transform Parent;

    public bool AttackAnimation = false;
    public bool MovingAnimation = false;
    public bool MoveOrder = false;

    protected override void Start()
    {
        base.Start();
        CharacterAttack.OnTergetDead.AddListener(OnTargetDead);
        CharacterAttack.OnEnemyExitRange.AddListener(OnEnemyExitRange);
        CharacterAttack.OnEnemyEnterRange.AddListener(OnEnemyEnterRange);
        CharacterMove.PathComplete.AddListener(PathComplete);
        StartCoroutine(ChaseEnemy(0.25f));
    }

    protected override void Update()
    {
        base.Update();

        if (CharacterMove.IsMove == true)
        { 
            if (MovingAnimation == false)
            {
                MovingAnimation = true;
                CharacterAnimator.StartMove();
            }
        }
        else
        {
            if (MovingAnimation == true)
            {
                MovingAnimation = false;
                CharacterAnimator.StopMove();
            }
        }

        if (CharacterAttack.TargetCollision == true & MoveOrder == false)
        {
            if (AttackAnimation == false)
            {
                AttackAnimation = true;
                CharacterAnimator.StartAttack();
                CharacterMove.Block = true;
            }
        }

        if (CharacterAttack.TargetCollision == true & MoveOrder == false)
        {
            Vector3 _difference = CharacterAttack.Target.position - Parent.position;
            _difference.Normalize();
            _difference.y = 0;
            Parent.rotation = Quaternion.Slerp(Parent.rotation, Quaternion.LookRotation(_difference), Time.deltaTime * 5);
        }
    }

    public void Attack(Transform _target)
    {
        MoveOrder = false;
        CharacterAnimator.StopAttack();
        AttackAnimation = false;
        CharacterAttack.DropTarget();
        CharacterAttack.Target = _target;
        CharacterAttack.CheckCollision();
    }

    public void MoveToPoint(Vector3 _point, bool _target)
    {
        if (_target == true)
        {
            _point = GetDelay(_point);
        }
        MoveOrder = true;
        CharacterAnimator.StopAttack();
        AttackAnimation = false;
        CharacterAttack.DropTarget();
        CharacterMove.Block = false;
        CharacterMove.MoveToPoint(_point);
    }

    public void FightAnimationEnd()
    {
        if (CharacterAttack.TargetCollision == true)
        {
            CharacterAttack.SimpleAttack();
        }
        else
        {
            CharacterMove.Block = false;
            AttackAnimation = false;
            CharacterAnimator.StopAttack();
        }
    }

    private void OnTargetDead()
    {
        CharacterMove.Block = false;
        CharacterAnimator.StopAttack();
        AttackAnimation = false;
    }

    private void OnEnemyEnterRange()
    {
        if (MoveOrder != true)
        {
            CharacterMove.ResetPath();
        }
    }

    private void OnEnemyExitRange()
    {

    }

    private void PathComplete()
    {
        MoveOrder = false;
        CharacterAttack.AllowSetTarget();
    }

    private IEnumerator ChaseEnemy(float _delay)
    {
        while (true)
        {
            if (MoveOrder == false & CharacterAttack.Target != null & CharacterAttack.TargetCollision == false)
            {
                CharacterMove.MoveToPoint(GetDelay(CharacterAttack.Target.position));
            }
            yield return new WaitForSeconds(_delay);
        }
    }

    private Vector3 GetDelay(Vector3 _target)
    {
        return _target - (_target - transform.position).normalized * CharacterMove.Speed * Time.deltaTime;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
