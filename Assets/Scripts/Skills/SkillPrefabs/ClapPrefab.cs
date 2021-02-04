using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClapPrefab : MonoBehaviour
{
    [SerializeField] private Rigidbody Rigidbody;
    [SerializeField] private DamageType _damageType;
    private int _damage;

    public void Init(int _damageValue)
    {
        _damage = _damageValue;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(GameManager.Instance.PlayersTag))
            other.attachedRigidbody.GetComponent<IDamageable>().ApplyDamage(_damageType, _damage);
    }
}
