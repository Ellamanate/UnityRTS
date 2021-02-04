using UnityEngine;
using UnityEditor;


public class ForceWave : ActiveSkill
{
    public int Damage;
    public float Force;
    public DamageWavePrefab Wave;

    public ForceWave() { SkillTargetSelector = new SelectVector(this); }

    public override void Effect(SkillCaster _caster, SkillTarget _target)
    {
        if (_target.TargetPoint != null)
        {
            DamageWavePrefab _wave = GameObject.Instantiate(Wave, _caster.transform.position, new Quaternion(0, 0, 0, 0));
            _wave.Init(Damage, ((Vector3)(_target.TargetPoint - _caster.transform.position)).normalized * Force);
        }
    }

    [MenuItem("Assets/Create/Skills/ActiveSkill/ForceWave", false, 2)]
    public static void Create()
    {
        ForceWave original = ScriptableObject.CreateInstance<ForceWave>();
        SaveInstance(original, "Active/ForceWave");
    }
}