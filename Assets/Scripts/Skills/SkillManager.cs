using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class SkillManager
{
    public object SkillTarget;
    public SkillCaster Caster { get => _caster; }
    public SkillData CurrentSkillData { get => _currentSkillData; }

    private SkillCaster _caster;
    private SkillData _currentSkillData;
    private SkillTargetSelector _skillTargetSelector;

    public SkillManager(SkillCaster _initCaster, IReadOnlyCollection<SkillData> _skills) 
    {
        _caster = _initCaster;

        for (int i = 0; i < _caster.Skills.Count; i++)
        {
            _caster.Skills.ElementAt(i).Ready = true;
            _caster.Skills.ElementAt(i).Skill.OnStart(Caster);
        }
    }

    public void TryCastSkill(int index)
    {
        if (_caster.Skills.Count > index)
        {
            _currentSkillData = _caster.Skills.ElementAt(index);
            _skillTargetSelector = _currentSkillData.Skill.SkillTargetSelector;
            TryCastSkill();
        }
    }

    public void TryCastSkill(SkillData _skillData)
    {
        if (_skillData != null)
        {
            _currentSkillData = _skillData;
            _skillTargetSelector = _currentSkillData.Skill.SkillTargetSelector;
            TryCastSkill();
        }
    }

    public void SelectTarget()
    {
        _skillTargetSelector.SelectTarget(this);
    }

    public void CastSkill()
    {
        _currentSkillData.Skill.Activate(Caster, SkillTarget);
        StartTimer(_currentSkillData.Skill.CoolDown);
        Events.OnSkillActivate.Publish(this);
    }

    private void StartTimer(float _time)
    {
        IEnumerator _coroutine = _currentSkillData.StartTimer(_time);
        GameManager.Instance.CreateCoroutine(_coroutine);
    }

    private void TryCastSkill()
    {
        if (_currentSkillData.Ready & _currentSkillData.Skill.CanActivate)
        {
            if (_currentSkillData.Skill.SelectingTarget)
                UnitManager.Instance.SkillTargetSelecting(this);
            else
                SelectTarget();
        }
    }
}
