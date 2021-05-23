using UnityEngine;


namespace Skills
{
    public class SkillManager : MonoBehaviour
    {
        [SerializeField] private SkillData[] _skills;
        [SerializeField] private int _maxMP;
        [SerializeField] private int _currentMP;
        private SkillCaster _skillManager;

        public int MaxMP { get => _maxMP; set => OnChangeMaxMP(value); }
        public int CurrentMP { get => _currentMP; set => OnChangeCurrentMP(value); }
        public SkillData GetSkillByIndex(int index) => _skills[index];
        public int GetSkillsCount() => _skills.Length;
        public void CastSkill(object skillTarget) => _skillManager.CastSkill(this, skillTarget);
        public void SelectSkillTarget(int skillIndex) => _skillManager.SelectSkillTarget(this, skillIndex);
        public void SelectSkillTarget(SkillData skillData) => _skillManager.SelectSkillTarget(this, skillData);
        public void SelectTarget() => _skillManager.SelectTarget(this);

        private void Start()
        {
            _skillManager = new SkillCaster((SkillData[])_skills.Clone());
            CurrentMP = MaxMP;

            InitSkills();
        }

        private void OnChangeCurrentMP(int _value)
        {
            if (_value > _currentMP)
                _currentMP = _maxMP;
            else
                _currentMP = _value;

            Events.OnSkillCasterMPChange.Publish(this);
        }

        private void OnChangeMaxMP(int _value)
        {
            _maxMP = _value;

            if (_currentMP > _value)
                _currentMP = _value;

            Events.OnSkillCasterMPChange.Publish(this);
        }

        private void InitSkills()
        {
            for (int i = 0; i < _skills.Length; i++)
            {
                _skills[i].Ready = true;
                _skills[i].Skill.OnStart(this);
            }
        }
    }
}