using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class SkillManager
{
    public SkillCaster Caster;
    public List<SkillData> Skills;
    public SkillData CurrentSkillData;
    public SkillTarget SkillTarget;
    private SkillTargetSelector _skillTargetSelector;

    public SkillManager(SkillCaster _caster, List<SkillData> _skills) 
    {
        Caster = _caster;
        Skills = _skills;
        SkillTarget = new SkillTarget();

        for (int i = 0; i < Skills.Count; i++)
        {
            Skills[i].Ready = true;
            Skills[i].Skill.OnStart(Caster);
        }
    }

    public void TryCastSkill(int index)
    {
        if (Skills.Count > index)
        {
            CurrentSkillData = Skills[index];
            _skillTargetSelector = CurrentSkillData.Skill.SkillTargetSelector;
            TryCastSkill();
        }
    }

    public void TryCastSkill(SkillData _skillData)
    {
        if (_skillData != null)
        {
            CurrentSkillData = _skillData;
            _skillTargetSelector = CurrentSkillData.Skill.SkillTargetSelector;
            TryCastSkill();
        }
    }

    public void SelectTarget()
    {
        _skillTargetSelector.SelectTarget(this);
    }

    public void CastSkill()
    {
        CurrentSkillData.Skill.Activate(Caster, SkillTarget);
        StartTimer(CurrentSkillData.Skill.CoolDown);
        Events.OnSkillActivate.Publish(this);
    }

    private void StartTimer(float _time)
    {
        IEnumerator _coroutine = CurrentSkillData.StartTimer(_time);
        GameManager.Instance.CreateCoroutine(_coroutine);
    }

    private void TryCastSkill()
    {
        if (CurrentSkillData.Ready & CurrentSkillData.Skill.CanActivate)
        {
            if (CurrentSkillData.Skill.SelectingTarget)
                UnitManager.Instance.SkillTargetSelecting(this);
            else
                SelectTarget();
        }
    }
}

public class SkillTarget
{
    public IDamageable Body;
    public Vector3? TargetPoint;

    public void SetBody(IDamageable _body)
    {
        Body = _body;
        TargetPoint = null;
    }

    public void SetTargrtPoint(Vector3 _targetPoint)
    {
        TargetPoint = _targetPoint;
        Body = null;
    }
}

[System.Serializable]
public class SkillData
{
    public ProtoSkill Skill;
    [HideInInspector] public int Timer;
    [HideInInspector] public bool Ready;
    [HideInInspector] public UnityAction Tick;

    public IEnumerator StartTimer(float _time)
    {
        Timer = Mathf.RoundToInt(_time * 10);
        Ready = false;

        while (Timer > 0)
        {
            yield return new WaitForSeconds(0.1f);
            Timer -= 1;

            if (Tick != null)
                Tick.Invoke();
        }

        Ready = true;
    }
}