using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator: MonoBehaviour
{
    public bool AttackAnimation { get => _attackAnimation; }
    public bool MovingAnimation { get => _movingAnimation; }
    public bool DeadAnimation { get => _deadAnimation; }

    [SerializeField] private Animator Animator;
    [SerializeField] private bool _attackAnimation = false;
    [SerializeField] private bool _movingAnimation = false;
    [SerializeField] private bool _deadAnimation = false;
    private bool _lockAnimation = false;

    public void StartMove()
    {
        if (!_lockAnimation)
        {
            _movingAnimation = true;
            Animator.SetBool("Walk", true);
        }
    }

    public void StopMove()
    {
        _movingAnimation = false;
        Animator.SetBool("Walk", false);
    }

    public void StartAttack()
    {
        if (!_lockAnimation)
        {
            _attackAnimation = true;
            Animator.SetBool("Fight", true);
        }
    }

    public void StopAttack()
    {
        _attackAnimation = false;
        Animator.SetBool("Fight", false);
    }

    public void StartDeath()
    {
        StopMove();
        StopAttack();
        _lockAnimation = true;
        Animator.SetBool("Death", true);
    }

    public void DeathAnimationEnd()
    {
        Destroy(gameObject);
    }

    public bool CheckAnimation(string _name)
    {
        return Animator.GetCurrentAnimatorStateInfo(0).IsName(_name);
    }

    public void ToDefaultState(float _time)
    {
        if (!_lockAnimation)
        {
            Animator.Rebind();
            Animator.Update(_time);
        }
    }
}
