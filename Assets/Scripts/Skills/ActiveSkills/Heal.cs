using UnityEngine;
using UnityEditor;


public class Heal : ActiveSkill
{
    public int HealValue { get => _healValue; }

    [SerializeField] private int _healValue;

    public Heal() : base(new SelectBody()) { }

    public override void Effect(SkillCaster _caster, object _target)
    {
        if (TypeChecker<IDamageable>.CheckObject(_target, out IDamageable _damageable))
            _damageable.CurrentHP += _healValue;
    }

    [MenuItem("Assets/Create/Skills/ActiveSkill/Heal", false, 1)]
    public static void Create()
    {
        Heal original = ScriptableObject.CreateInstance<Heal>();
        SaveInstance(original, "Active/Heal");
    }
}