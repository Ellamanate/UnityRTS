using UnityEditor;
using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "Damage", menuName = "Damage", order = 1)]
public class DamageType : ScriptableObject
{
    [SerializeField] private List<Damage> DamageForArmorType = new List<Damage>();

    public int DecreaseDamage(ArmorType _currentArmorType, int _damageValue) 
    { 
        foreach (Damage _damage in DamageForArmorType)
        {
            if (_damage.ArmorType == _currentArmorType)
                return Mathf.RoundToInt(_damage.DamageByArmor * _damageValue);
        }

        return _damageValue;
    }
}

[System.Serializable]
public class Damage
{
    public ArmorType ArmorType;
    [Range(0, 1)] public float DamageByArmor;
}
