using UnityEngine;
using System.Collections.Generic;


public class SkillCaster : MonoBehaviour
{
    public IReadOnlyCollection<SkillData> Skills { get => _skills.AsReadOnly(); }
    public int MaxMP { get => _maxMP; set => OnChangeMaxMP(value); }
    public int CurrentMP { get => _currentMP; set => OnChangeCurrentMP(value); }
    public SkillManager SkillManager { get => _skillManager; }

    [SerializeField] private List<SkillData> _skills;
    [SerializeField] private int _maxMP;
    [SerializeField] private int _currentMP;
    private SkillManager _skillManager;

    private void Start()
    {
        _skillManager = new SkillManager(this, Skills);
        CurrentMP = MaxMP;
    }

    private void OnChangeCurrentMP(int _value)
    {
        if (_value > _currentMP)
            _currentMP = _maxMP;
        else
            _currentMP = _value;

        Events.OnSkillCasterMPChange.Publish(this);
    }

    private void OnChangeMaxMP(int _value)
    {
        _maxMP = _value;

        if (_currentMP > _value)
            _currentMP = _value;

        Events.OnSkillCasterMPChange.Publish(this);
    }
}