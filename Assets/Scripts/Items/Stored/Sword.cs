using UnityEditor;
using UnityEngine;


public class Sword : StoredItem
{
    [SerializeField] private int _damageValue;

    public override void Activate(Unit _owner, ItemPrefab _prefab)
    {
        if (_owner.GetComponent<Character>() != null)
            _owner.GetComponent<Character>().AddDamage(_damageValue);
    }

    public override void Drop(Backpack _backpack, ItemPrefab _prefab)
    {
        base.Drop(_backpack, _prefab);
        if (_backpack.GetComponent<Character>() != null)
            _backpack.GetComponent<Character>().RemoveDamage(_damageValue);
    }

    [MenuItem("Assets/Create/Items/Stored/Sword", false, 1)]
    public static void Create()
    {
        Sword original = ScriptableObject.CreateInstance<Sword>();
        SaveInstance(original, "Stored/Sword");
    }
}