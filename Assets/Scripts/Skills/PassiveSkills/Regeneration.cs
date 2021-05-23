using UnityEditor;
using UnityEngine;
using System.Collections;


namespace Skills
{
    public class Regeneration : PassiveSkill
    {
        public int HealValue => _healValue;

        [SerializeField] private int _healValue;

        public override void OnStart(SkillManager caster)
        {
            IEnumerator heal = Heal(caster);
            GameManager.Instance.CreateCoroutine(heal);
        }

        public IEnumerator Heal(SkillManager caster)
        {
            yield return new WaitForSeconds(CoolDown);

            while (caster != null)
            {
                if (TypeChecker<Unit>.CheckGameObject(caster.gameObject, out Unit unit))
                    unit.CurrentHP += _healValue;

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
}