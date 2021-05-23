using UnityEngine;


namespace Skills
{
    public abstract class SkillTargetSelector
    {
        public virtual bool IsSelectingTarget() => false;
        public virtual void SelectTarget(SkillManager skillCaster) { }
    }

    public class SelectNobody : SkillTargetSelector
    {
        public override bool IsSelectingTarget() => false;

        public override void SelectTarget(SkillManager skillCaster)
        {
            skillCaster.CastSkill(skillCaster.transform.position);
        }
    }

    public class SelectBody : SkillTargetSelector
    {
        public override bool IsSelectingTarget() => true;

        public override void SelectTarget(SkillManager skillCaster)
        {
            if (Raycaster.Instance.SelectMouseRaycast(out Rigidbody _attachedRigidbody))
            {
                skillCaster.CastSkill(_attachedRigidbody.GetComponent<IDamageable>());
                return;
            }

            Debug.Log("Не верная цель");
        }
    }

    public class SelectVector : SkillTargetSelector
    {
        public override bool IsSelectingTarget() => true;

        public override void SelectTarget(SkillManager skillCaster)
        {
            if (Raycaster.Instance.DefaultMouseRaycast(out RaycastHit _hitInfo))
            {
                skillCaster.CastSkill(_hitInfo.point);
                return;
            }

            Debug.Log("Не верная цель");
        }
    }
}