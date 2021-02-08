using UnityEngine;


public abstract class SkillTargetSelector
{
    public virtual bool IsSelectingTarget() { return false; }
    public virtual void SelectTarget(SkillManager _skillManager) { }
}

public class SelectNobody : SkillTargetSelector
{
    public override bool IsSelectingTarget() { return false; }

    public override void SelectTarget(SkillManager _skillManager)
    {
        _skillManager.SkillTarget = _skillManager.Caster.transform.position;
        _skillManager.CastSkill();
    }
}

public class SelectBody : SkillTargetSelector
{
    public override bool IsSelectingTarget() { return true; }

    public override void SelectTarget(SkillManager _skillManager)
    {
        if (Raycaster.Instance.SelectMouseRaycast(out Rigidbody _attachedRigidbody))
        {
            _skillManager.SkillTarget = _attachedRigidbody.GetComponent<IDamageable>();
            _skillManager.CastSkill();
            return;
        }

        Debug.Log("Не верная цель");
    }
}

public class SelectVector : SkillTargetSelector
{
    public override bool IsSelectingTarget() { return true; }

    public override void SelectTarget(SkillManager _skillManager)
    {
        if (Raycaster.Instance.DefaultMouseRaycast(out RaycastHit _hitInfo))
        {
            _skillManager.SkillTarget = _hitInfo.point;
            _skillManager.CastSkill();
            return;
        }

        Debug.Log("Не верная цель");
    }
}