using UnityEngine;


public class SkillTargetSelector
{
    public SkillTargetSelector(ProtoSkill _skill, bool _selectingTarget)
    {
        _skill.SelectingTarget = _selectingTarget;
    }

    public virtual void SelectTarget(SkillManager _skillManager) { }
}

public class SelectNobody : SkillTargetSelector
{
    public SelectNobody(ProtoSkill _skill) : base(_skill, false) { }

    public override void SelectTarget(SkillManager _skillManager)
    {
        _skillManager.SkillTarget.SetTargrtPoint(_skillManager.Caster.transform.position);
        _skillManager.CastSkill();
    }
}

public class SelectBody : SkillTargetSelector
{
    public SelectBody(ProtoSkill _skill) : base(_skill, true) { }

    public override void SelectTarget(SkillManager _skillManager)
    {
        if (Raycaster.Instance.SelectMouseRaycast(out Rigidbody _attachedRigidbody))
        {
            _skillManager.SkillTarget.SetBody(_attachedRigidbody.GetComponent<IDamageable>());
            _skillManager.CastSkill();
            return;
        }

        Debug.Log("Не верная цель");
    }
}

public class SelectVector : SkillTargetSelector
{
    public SelectVector(ProtoSkill _skill) : base(_skill, true) { }

    public override void SelectTarget(SkillManager _skillManager)
    {
        if (Raycaster.Instance.DefaultMouseRaycast(out RaycastHit _hitInfo))
        {
            _skillManager.SkillTarget.SetTargrtPoint(_hitInfo.point);
            _skillManager.CastSkill();
            return;
        }

        Debug.Log("Не верная цель");
    }
}