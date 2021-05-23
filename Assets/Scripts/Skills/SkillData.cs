using System.Collections;
using UnityEngine;
using UnityEngine.Events;


namespace Skills
{
    [System.Serializable]
    public class SkillData
    {
        public UnityAction Tick;
        [SerializeField] private ProtoSkill _skill;
        private float _timer;
        private bool _ready;

        public ProtoSkill Skill => _skill;
        public float Timer => _timer;
        public bool Ready { get => _ready; set { _ready = value; _timer = 0; } }

        public IEnumerator StartTimer(float time)
        {
            _timer = time;
            _ready = false;

            while (_timer > 0)
            {
                yield return new WaitForSeconds(0.1f);
                _timer -= 0.1f;

                if (Tick != null)
                    Tick.Invoke();
            }

            _ready = true;
        }
    }
}