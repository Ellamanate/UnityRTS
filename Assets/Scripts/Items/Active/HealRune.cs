using UnityEditor;
using UnityEngine;


public class HealRune : ActiveItem
{
    [SerializeField] private int _healValue;

    public override void Activate(Unit _owner, ItemPrefab _prefab)
    {
        _owner.CurrentHP += _healValue;
    }

    [MenuItem("Assets/Create/Items/Active/HealRune", false, 1)]
    public static void Create()
    {
        HealRune original = ScriptableObject.CreateInstance<HealRune>();
        SaveInstance(original, "Activate/HealRune");
    }
}