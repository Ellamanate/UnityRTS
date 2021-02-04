using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class SkillUI : MonoBehaviour, IPointerClickHandler
{
    public Image Icon;
    public Text Counter;
    [HideInInspector] public SkillData SkillData;
    [HideInInspector] public SkillManager SkillManager;

    public void UpdateCounter()
    {
        if (SkillData != null & this != null)
        {
            if (SkillData.Timer == 0)
                Counter.text = "";
            else
                Counter.text = (Mathf.Round(SkillData.Timer) / 10).ToString();
        }
    }

    public void OnPointerClick(PointerEventData _pointerEventData)
    {
        SkillManager.TryCastSkill(SkillData);
    }
}