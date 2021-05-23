using UnityEngine;
using UnityEditor;


namespace Skills
{
    public class Heal : ActiveSkill
    {
        public int HealValue { get => _healValue; }

        [SerializeField] private int _healValue;

        public Heal() : base(new SelectBody()) { }

        public override void Effect(SkillManager caster, object target)
        {
            if (TypeChecker<IDamageable>.CheckObject(target, out IDamageable damageable))
                damageable.CurrentHP += _healValue;
        }

        [MenuItem("Assets/Create/Skills/ActiveSkill/Heal", false, 1)]
        public static void Create()
        {
            Heal original = ScriptableObject.CreateInstance<Heal>();
            SaveInstance(original, "Active/Heal");
        }
    }
}