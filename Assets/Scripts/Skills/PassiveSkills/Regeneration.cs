using UnityEditor;
using UnityEngine;
using System.Collections;


public class Regeneration : PassiveSkill
{
    public int HealValue { get => _healValue; }

    [SerializeField] private int _healValue;

    public override void OnStart(SkillCaster _caster)
    {
        IEnumerator _heal = Heal(_caster);
        GameManager.Instance.CreateCoroutine(_heal);
    }

    public IEnumerator Heal(SkillCaster _caster)
    {
        yield return new WaitForSeconds(CoolDown);

        while (_caster != null)
        {
            if (TypeChecker<Unit>.CheckGameObject(_caster.gameObject, out Unit _unit))
                _unit.CurrentHP += _healValue;

            yield return new WaitForSeconds(CoolDown);
        }
    }

    [MenuItem("Assets/Create/Skills/PassiveSkill/Regeneration", false, 1)]
    public static void Create()
    {
        Regeneration original = ScriptableObject.CreateInstance<Regeneration>();
        SaveInstance(original, "Passive/Regeneration");
    }
}