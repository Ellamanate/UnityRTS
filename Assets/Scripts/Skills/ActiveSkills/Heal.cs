using UnityEngine;
using UnityEditor;


public class Heal : ActiveSkill
{
    public int HealValue;

    public Heal() { SkillTargetSelector = new SelectBody(this); }

    public override void Effect(SkillCaster _caster, SkillTarget _target)
    {
        if (_target.Body != null)
        {
            _target.Body.CurrentHP += HealValue;
        }
    }

    [MenuItem("Assets/Create/Skills/ActiveSkill/Heal", false, 1)]
    public static void Create()
    {
        Heal original = ScriptableObject.CreateInstance<Heal>();
        SaveInstance(original, "Active/Heal");
    }
}