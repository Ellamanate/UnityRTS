using UnityEngine;
using UnityEditor;
using System;


public abstract class ProtoSkill : ScriptableObject
{
    public int CoolDown { get => _coolDown; set => _coolDown = OnCoolDownChange(value); }
    public bool SelectingTarget { get => _selectingTarget; }
    public bool CanActivate { get => _canActivate; }
    public SkillTargetSelector SkillTargetSelector { get => _skillTargetSelector; }

    [SerializeField] private int _coolDown;
    private bool _selectingTarget;
    private bool _canActivate;
    private SkillTargetSelector _skillTargetSelector;

    public ProtoSkill(SkillTargetSelector _initSkillTargetSelector, bool _initCanActivate)
    {
        _canActivate = _initCanActivate;
        _skillTargetSelector = _initSkillTargetSelector;
        _selectingTarget = _skillTargetSelector.IsSelectingTarget();
    }

    public virtual void OnStart(SkillCaster _caster) { }

    public virtual void Activate(SkillCaster _caster, object _target) { }

    protected static void SaveInstance(ScriptableObject _obj, string _name) 
    {
        string _uniqueFileName = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/Skills/" + _name + ".asset");
        AssetDatabase.CreateAsset(_obj, _uniqueFileName);
        AssetDatabase.Refresh();
    }

    private int OnCoolDownChange(int _value)
    {
        if (_value < 0)
            new Exception("Trying set CoolDown < 0");

        return _value;
    }   
}