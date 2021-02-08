using System.Collections;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class SkillData
{
    public UnityAction Tick;
    public ProtoSkill Skill { get => _skill; }
    public int Timer { get => _timer; }
    public bool Ready { get => _ready; set { _ready = value; _timer = 0; }}

    [SerializeField] private ProtoSkill _skill;
    private int _timer;
    private bool _ready;

    public IEnumerator StartTimer(float _time)
    {
        _timer = Mathf.RoundToInt(_time * 10);
        _ready = false;

        while (_timer > 0)
        {
            yield return new WaitForSeconds(0.1f);
            _timer -= 1;

            if (Tick != null)
                Tick.Invoke();
        }

        _ready = true;
    }
}