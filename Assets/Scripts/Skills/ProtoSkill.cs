using UnityEngine;
using UnityEditor;


public abstract class ProtoSkill : ScriptableObject
{
    public int CoolDown;
    [HideInInspector] public bool SelectingTarget;
    [HideInInspector] public bool CanActivate;
    [HideInInspector] public SkillTargetSelector SkillTargetSelector;

    public ProtoSkill() { }

    public virtual void OnStart(SkillCaster _caster) { }

    public virtual void Activate(SkillCaster _caster, SkillTarget _target) { }

    protected static void SaveInstance(ScriptableObject _obj, string _name) 
    {
        string _uniqueFileName = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/Skills/" + _name + ".asset");
        AssetDatabase.CreateAsset(_obj, _uniqueFileName);
        AssetDatabase.Refresh();
    }
}