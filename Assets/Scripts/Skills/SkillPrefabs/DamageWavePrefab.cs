using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageWavePrefab : MonoBehaviour
{
    [SerializeField] private Rigidbody Rigidbody;
    [SerializeField] private DamageType _damageType;
    [SerializeField] private float _timeToLife;
    private int _damage;

    public void Init(int _damageValue, Vector3 _velocity)
    {
        _damage = _damageValue;
        Rigidbody.velocity = _velocity;
    }

    private void Start()
    {
        Destroy(gameObject, _timeToLife);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(GameManager.Instance.PlayersTag))
            other.attachedRigidbody.GetComponent<IDamageable>().ApplyDamage(_damageType, _damage);
    }
}
