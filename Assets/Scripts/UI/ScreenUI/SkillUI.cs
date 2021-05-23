using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Skills;


public class SkillUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Text _counter;
    [SerializeField] private Image _image;
    private SkillData _skillData;
    private SkillManager _skillCaster;

    public void UpdateCounter()
    {
        if (_skillData != null & this != null)
        {
            if (_skillData.Timer <= 0)
                _counter.text = string.Empty;
            else
                _counter.text = new DateTime().AddSeconds(_skillData.Timer).ToString("s:f");
        }
    }

    public void SetSkillManager(SkillManager skillCaster, int index)
    {
        _skillCaster = skillCaster;
        _skillData = _skillCaster.GetSkillByIndex(index);
        _image.sprite = _skillData.Skill.Sprite;
        UpdateCounter();
    }

    public void OnPointerClick(PointerEventData _pointerEventData) => _skillCaster.SelectSkillTarget(_skillData);
}