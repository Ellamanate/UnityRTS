using UnityEditor;
using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "Damage", menuName = "Damage", order = 1)]
public class DamageType : ScriptableObject
{
    public List<Damage> DamageForArmorType = new List<Damage>();

    public int DecreaseDamage(Armor _armor, int _damageValue) 
    { 
        foreach (Damage _damage in DamageForArmorType)
        {
            if (_damage.ArmorType == _armor)
                return Mathf.RoundToInt(_damage.DamageByArmor * _damageValue);
        }

        return _damageValue;
    }
}

[System.Serializable]
public class Damage
{
    public Armor ArmorType;
    [Range(0, 1)] public float DamageByArmor;
}
