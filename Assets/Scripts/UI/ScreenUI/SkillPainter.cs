using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Skills;


public class SkillPainter 
{
    private ObjectPool<SkillUI> _skillsUIPool;
    private List<SkillUI> _skills = new List<SkillUI>();
    private SkillManager _currentSkillCaster;
    private RectTransform _skillsPanel;
    private Image _manaBar;

    public SkillPainter(SkillUI skillPrefab, RectTransform pool, RectTransform skillPanel, Image manaBar)
    {
        _skillsUIPool = new ObjectPool<SkillUI>(skillPrefab, pool);
        _skillsPanel = skillPanel;
        _manaBar = manaBar;
    }

    public void PaintSkills(Unit selectedUnit)
    {
        if (selectedUnit != null)
        {
            if (TypeChecker<SkillManager>.CheckGameObject(selectedUnit.gameObject, out SkillManager caster))
            {
                SkillUI[] skillParents = _skillsUIPool.GetObjects(caster.GetSkillsCount());
                _skills.Clear();

                for (int i = 0; i < skillParents.Length; i++)
                {
                    skillParents[i].transform.SetParent(_skillsPanel);
                    skillParents[i].SetSkillManager(caster, i);
                    _skills.Add(skillParents[i]);
                }

                if (caster != _currentSkillCaster)
                {
                    TickUnsubscribes();
                    TickSubscribes(caster);
                    _currentSkillCaster = caster;
                }

                UpdateManaBar(caster);
            }
            else
            {
                _skills.Clear();
                _skillsUIPool.BackToPoolAll();
                ResetManaBarToZero();
            }
        }
        else DropTarget();
    }

    public void DropTarget()
    {
        _skills.Clear();
        _skillsUIPool.BackToPoolAll();
        TickUnsubscribes();
        ResetManaBarToZero();

        _currentSkillCaster = null;
    }

    private void TickSubscribes(SkillManager caster)
    {
        if (caster != null)
        {
            for (int i = 0; i < _skills.Count; i++)
                caster.GetSkillByIndex(i).Tick += _skills[i].UpdateCounter;
        }
    }

    private void TickUnsubscribes()
    {
        if (_currentSkillCaster != null)
        {
            for (int i = 0; i < _skills.Count; i++)
                _currentSkillCaster.GetSkillByIndex(i).Tick -= _skills[i].UpdateCounter;
        }
    }

    public void UpdateManaBar(SkillManager caster) => _manaBar.fillAmount = (float)caster.CurrentMP / (float)caster.MaxMP;

    private void ResetManaBarToZero() => _manaBar.fillAmount = 0;
}