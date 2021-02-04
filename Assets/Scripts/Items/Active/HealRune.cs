using UnityEditor;
using UnityEngine;


public class HealRune : ActiveItem
{
    public int HealValue;

    public override void Activate(Unit _owner, ItemPrefab _prefab)
    {
        _owner.CurrentHP += HealValue;
    }

    [MenuItem("Assets/Create/Items/Active/HealRune", false, 1)]
    public static void Create()
    {
        HealRune original = ScriptableObject.CreateInstance<HealRune>();
        SaveInstance(original, "Activate/HealRune");
    }
}