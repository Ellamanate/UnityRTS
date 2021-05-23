using UnityEngine;


namespace Skills
{
    public class DamageWavePrefab : MonoBehaviour
    {
        [SerializeField] private Rigidbody Rigidbody;
        [SerializeField] private DamageType _damageType;
        [SerializeField] private float _timeToLife;
        private int _damage;

        public void Init(int damageValue, Vector3 velocity)
        {
            _damage = damageValue;
            Rigidbody.velocity = velocity;
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
}
