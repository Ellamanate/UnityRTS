using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator: MonoBehaviour
{
    public Animator Animator;

    public void StartMove()
    {
        Animator.SetBool("Walk", true);
    }

    public void StopMove()
    {
        Animator.SetBool("Walk", false);
    }

    public void StartAttack()
    {
        Animator.SetBool("Fight", true);
    }

    public void StopAttack()
    {
        
        Animator.SetBool("Fight", false);
    }

    public bool CheckAnimation(string _name)
    {
        return Animator.GetCurrentAnimatorStateInfo(0).IsName(_name);
    }
}
