using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActiveSkill : ProtoSkill
{
    public int ManaCost;

    public ActiveSkill() { CanActivate = true; }

    public override void Activate(SkillCaster _caster, SkillTarget _target)
    {
        if (_caster.CurrentMP >= ManaCost)
        {
            _caster.CurrentMP -= ManaCost;
            Effect(_caster, _target);
        }
    }

    public virtual void Effect(SkillCaster _caster, SkillTarget _target) { }
}


