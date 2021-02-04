using UnityEngine;
using UnityEditor;


public class Clap : ActiveSkill
{
    public int Damage;
    public ClapPrefab ClapPrefab;

    public Clap() { SkillTargetSelector = new SelectNobody(this); }

    public override void Effect(SkillCaster _caster, SkillTarget _target)
    {
        if (_target.TargetPoint != null)
        {
            ClapPrefab _clap = GameObject.Instantiate(ClapPrefab, _caster.transform.position, new Quaternion(0, 0, 0, 0));
            _clap.Init(Damage);
        }
    }

    [MenuItem("Assets/Create/Skills/ActiveSkill/Clap", false, 3)]
    public static void Create()
    {
        Clap original = ScriptableObject.CreateInstance<Clap>();
        SaveInstance(original, "Active/Clap");
    }
}