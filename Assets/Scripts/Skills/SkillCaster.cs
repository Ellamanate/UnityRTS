using System.Collections;
using UnitManagement;


namespace Skills
{
    public class SkillCaster
    {
        public SkillData CurrentSkillData { get => _currentSkillData; }

        private SkillData _currentSkillData;
        private SkillTargetSelector _skillTargetSelector;
        private readonly SkillData[] _skills;

        public SkillCaster(SkillData[] skills) => _skills = skills;

        public void SelectSkillTarget(SkillManager caster, int skillIndex)
        {
            if (_skills.Length > skillIndex)
            {
                _currentSkillData = _skills[skillIndex];
                _skillTargetSelector = _currentSkillData.Skill.SkillTargetSelector;
                SelectSkillTarget(caster);
            }
        }

        public void SelectSkillTarget(SkillManager caster, SkillData skillData)
        {
            if (skillData != null)
            {
                _currentSkillData = skillData;
                _skillTargetSelector = _currentSkillData.Skill.SkillTargetSelector;
                SelectSkillTarget(caster);
            }
        }

        public void SelectTarget(SkillManager caster) => _skillTargetSelector.SelectTarget(caster);

        public void CastSkill(SkillManager caster, object skillTarget)
        {
            _currentSkillData.Skill.Activate(caster, skillTarget);
            StartTimer(_currentSkillData.Skill.CoolDown);
        }

        private void StartTimer(float time)
        {
            IEnumerator coroutine = _currentSkillData.StartTimer(time);
            GameManager.Instance.CreateCoroutine(coroutine);
        }

        private void SelectSkillTarget(SkillManager caster)
        {
            if (_currentSkillData.Ready & _currentSkillData.Skill.CanActivate)
            {
                if (_currentSkillData.Skill.SelectingTarget)
                    InputManager.Instance.SkillTargetSelecting(caster);
                else
                    SelectTarget(caster);
            }
        }
    }
}
