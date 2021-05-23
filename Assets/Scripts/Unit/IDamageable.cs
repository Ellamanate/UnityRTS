using System.Collections;
using UnityEngine;


public interface IDamageable
{
    int MaxHP { get; set;}
    
    int CurrentHP { get; set; }

    ArmorType ArmorType { get; set; }

    Collider HitBox { get; }

    GameObject GameObject { get; }

    void ApplyDamage(DamageType _damageType, int _applyedDamage);

    void Destroy();
}