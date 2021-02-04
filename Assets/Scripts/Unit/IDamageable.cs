using System.Collections;
using UnityEngine;


public interface IDamageable
{
    int MaxHP { get; set;}
    
    int CurrentHP { get; set; }

    Armor ArmorType { get; set; }

    Collider HitBox { get; }

    GameObject GameObject { get; }

    WorldUIContainer WorldUIContainer { get; }

    void ApplyDamage(DamageType _damageType, int _applyedDamage);

    void Destroy();
}