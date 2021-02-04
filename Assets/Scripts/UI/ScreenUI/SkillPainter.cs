using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class SkillPainter 
{
    private ObjectPool _skillsUIPool;
    private List<SkillUI> _skills = new List<SkillUI>();
    private SkillManager _currentSkillManager;
    private RectTransform _skillsPanel;
    private Image _manaBar;

    public SkillPainter(SkillUI _skillPrefab, RectTransform _pool, RectTransform _panel, Image _mana)
    {
        _skillsUIPool = new ObjectPool(_skillPrefab.gameObject, _pool);
        _skillsPanel = _panel;
        _manaBar = _mana;
    }

    public void PaintSkills(Unit _selectedUnit)
    {
        if (_selectedUnit != null)
        {
            if (_selectedUnit.GetComponent<SkillCaster>() != null)
            {
                SkillManager _skillManager = _selectedUnit.GetComponent<SkillCaster>().SkillManager;
                Transform[] _skillParents = _skillsUIPool.GetObjects(_skillManager.Skills.Count);
                _skills.Clear();

                for (int i = 0; i < _skillParents.Length; i++)
                {
                    _skillParents[i].SetParent(_skillsPanel);
                    _skills.Add(_skillParents[i].GetComponent<SkillUI>());
                }

                UpdateSkillsCoolDown(_skillManager);
                UpdateManaBar(_selectedUnit.GetComponent<SkillCaster>());
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
        _currentSkillManager = null;
        UpdateSkillsCoolDown(_currentSkillManager);
        ResetManaBarToZero();
    }

    public void UpdateSkillsCoolDown(SkillManager _skillManager)
    {
        if (_skillManager != _currentSkillManager)
        {
            for (int i = 0; i < _skills.Count; i++)
                _skillManager.Skills[i].Tick -= _skills[i].UpdateCounter;

            _currentSkillManager = _skillManager;

            for (int i = 0; i < _skills.Count; i++)
            {
                _skills[i].SkillData = _skillManager.Skills[i];
                _skills[i].SkillManager = _skillManager;
                _skills[i].UpdateCounter();
                _skillManager.Skills[i].Tick += _skills[i].UpdateCounter;
            }
        }
    }

    public void UpdateManaBar(SkillCaster _unit)
    {
        _manaBar.fillAmount = (float)_unit.CurrentMP / (float)_unit.MaxMP;
    }

    private void ResetManaBarToZero()
    {
        _manaBar.fillAmount = 0;
    }
}