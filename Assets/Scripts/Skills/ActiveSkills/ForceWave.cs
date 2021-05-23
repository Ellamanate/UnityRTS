using UnityEngine;
using UnityEditor;


namespace Skills
{
    public class ForceWave : ActiveSkill
    {
        public int DamageValue { get => _damageValue; }
        public int Force { get => _force; }
        public DamageWavePrefab Wave { get => _wave; }

        [SerializeField] private int _damageValue;
        [SerializeField] private int _force;
        [SerializeField] private DamageWavePrefab _wave;

        public ForceWave() : base(new SelectVector()) { }

        public override void Effect(SkillManager caster, object target)
        {
            if (TypeChecker<Vector3>.CheckObject(target, out Vector3 vector))
            {
                DamageWavePrefab _wave = GameObject.Instantiate(Wave, caster.transform.position, new Quaternion(0, 0, 0, 0));
                _wave.Init(_damageValue, (vector - caster.transform.position).normalized * Force);
            }
        }

        [MenuItem("Assets/Create/Skills/ActiveSkill/ForceWave", false, 2)]
        public static void Create()
        {
            ForceWave original = ScriptableObject.CreateInstance<ForceWave>();
            SaveInstance(original, "Active/ForceWave");
        }
    }
}