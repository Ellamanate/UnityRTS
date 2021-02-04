using UnityEngine;
using System.Collections.Generic;


public class SkillCaster : MonoBehaviour
{
    public List<SkillData> Skills;
    [HideInInspector] public SkillManager SkillManager;
    [SerializeField] private int _maxMP;
    [SerializeField] private int _currentMP;

    public int MaxMP
    {
        get { return _maxMP; }
        set
        {
            _maxMP = value;

            if (_currentMP > value)
                _currentMP = value;

            Events.OnSkillCasterMPChange.Publish(this);
        }
    }

    public int CurrentMP
    {
        get { return _currentMP; }
        set
        {
            if (value > MaxMP)
            {
                _currentMP = MaxMP;
                return;
            }
            else
                _currentMP = value;

            Events.OnSkillCasterMPChange.Publish(this);
        }
    }

    private void Start()
    {
        SkillManager = new SkillManager(this, Skills);
        CurrentMP = MaxMP;
    }
}