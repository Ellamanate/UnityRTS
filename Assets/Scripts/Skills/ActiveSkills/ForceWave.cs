using UnityEngine;
using UnityEditor;


public class ForceWave : ActiveSkill
{
    public int DamageValue { get => _damageValue; }
    public int Force { get => _force; }
    public DamageWavePrefab Wave { get => _wave; }

    [SerializeField] private int _damageValue;
    [SerializeField] private int _force;
    [SerializeField] private DamageWavePrefab _wave;

    public ForceWave() : base(new SelectVector()) { }

    public override void Effect(SkillCaster _caster, object _target)
    {
        if (TypeChecker<Vector3>.CheckObject(_target, out Vector3 _vector))
        {
            DamageWavePrefab _wave = GameObject.Instantiate(Wave, _caster.transform.position, new Quaternion(0, 0, 0, 0));
            _wave.Init(_damageValue, (_vector - _caster.transform.position).normalized * Force);
        }
    }

    [MenuItem("Assets/Create/Skills/ActiveSkill/ForceWave", false, 2)]
    public static void Create()
    {
        ForceWave original = ScriptableObject.CreateInstance<ForceWave>();
        SaveInstance(original, "Active/ForceWave");
    }
}