using UnityEngine;


public abstract class ActiveSkill : ProtoSkill
{
    public int ManaCost { get => _manaCost; }

    [SerializeField] private int _manaCost;

    public ActiveSkill(SkillTargetSelector _skillTargetSelector) : base(_skillTargetSelector, true) { }

    public override void Activate(SkillCaster _caster, object _target)
    {
        if (_caster.CurrentMP >= _manaCost)
        {
            _caster.CurrentMP -= _manaCost;
            Effect(_caster, _target);
        }
    }

    public virtual void Effect(SkillCaster _caster, object _target) { }
}


