using UnityEngine;


namespace Skills
{
    public abstract class ActiveSkill : ProtoSkill
    {
        public int ManaCost { get => _manaCost; }

        [SerializeField] private int _manaCost;

        public ActiveSkill(SkillTargetSelector skillTargetSelector) : base(skillTargetSelector, true) { }

        public override void Activate(SkillManager caster, object target)
        {
            if (caster.CurrentMP >= _manaCost)
            {
                caster.CurrentMP -= _manaCost;
                Effect(caster, target);
            }
        }

        public virtual void Effect(SkillManager caster, object target) { }
    }
}