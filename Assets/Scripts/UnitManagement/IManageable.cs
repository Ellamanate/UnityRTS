using UnityEngine;


namespace UnitManagement
{
    public interface IManageable
    {
        void Move(Vector3 point);
        void Move(IDamageable damageable);
        void Attack(Vector3 point);
        void Attack(IDamageable damageable);
        void Collect(ItemPrefab item);
        void Stop();
        void HoldPosition();
    }
}
