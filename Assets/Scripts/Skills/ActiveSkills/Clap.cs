using UnityEngine;
using UnityEditor;


namespace Skills
{
    public class Clap : ActiveSkill
    {
        public int DamageValue { get => _damageValue; }
        public ClapPrefab ClapPrefab { get => _clapPrefab; }

        [SerializeField] private int _damageValue;
        [SerializeField] private ClapPrefab _clapPrefab;

        public Clap() : base(new SelectNobody()) { }

        public override void Effect(SkillManager caster, object target)
        {
            if (TypeChecker<Vector3>.CheckObject(target, out Vector3 vector))
            {
                ClapPrefab _clap = GameObject.Instantiate(_clapPrefab, vector, new Quaternion(0, 0, 0, 0));
                _clap.Init(_damageValue);
            }
        }

        [MenuItem("Assets/Create/Skills/ActiveSkill/Clap", false, 3)]
        public static void Create()
        {
            Clap original = ScriptableObject.CreateInstance<Clap>();
            SaveInstance(original, "Active/Clap");
        }
    }
}