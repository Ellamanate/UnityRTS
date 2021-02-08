using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;


public class SkillUI : MonoBehaviour, IPointerClickHandler
{
    public SkillManager SkillManager { get => _skillManager; }
    public SkillData SkillData { get => _skillData; }

    [SerializeField] private Text _counter;
    private SkillData _skillData;
    private SkillManager _skillManager;

    public void UpdateCounter()
    {
        if (_skillData != null & this != null)
        {
            if (_skillData.Timer == 0)
                _counter.text = "";
            else
                _counter.text = (Mathf.Round(_skillData.Timer) / 10).ToString();
        }
    }

    public void SetSkillManager(SkillManager _newSkillManager, int _index)
    {
        _skillManager = _newSkillManager;
        _skillData = _skillManager.Caster.Skills.ElementAt(_index);
        UpdateCounter();
    }

    public void OnPointerClick(PointerEventData _pointerEventData)
    {
        _skillManager.TryCastSkill(_skillData);
    }
}