using UnityEngine;
using UnityEditor;
using System;


namespace Skills
{
    public abstract class ProtoSkill : ScriptableObject
    {
        [SerializeField] private Sprite _sprite;
        [SerializeField] private int _coolDown;
        private bool _selectingTarget;
        private bool _canActivate;
        private SkillTargetSelector _skillTargetSelector;

        public int CoolDown { get => _coolDown; set => _coolDown = OnCoolDownChange(value); }
        public bool SelectingTarget => _selectingTarget;
        public bool CanActivate => _canActivate;
        public SkillTargetSelector SkillTargetSelector => _skillTargetSelector;
        public Sprite Sprite => _sprite;

        public ProtoSkill(SkillTargetSelector skillTargetSelector, bool canActivate)
        {
            _canActivate = canActivate;
            _skillTargetSelector = skillTargetSelector;
            _selectingTarget = _skillTargetSelector.IsSelectingTarget();
        }

        public virtual void OnStart(SkillManager caster) { }

        public virtual void Activate(SkillManager caster, object target) { }

        protected static void SaveInstance(ScriptableObject obj, string name)
        {
            string _uniqueFileName = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/Skills/" + name + ".asset");
            AssetDatabase.CreateAsset(obj, _uniqueFileName);
            AssetDatabase.Refresh();
        }

        private int OnCoolDownChange(int value)
        {
            if (value < 0)
                new Exception("Trying set CoolDown < 0");

            return value;
        }
    }
}