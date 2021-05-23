using UnityEngine;


namespace UnitManagement
{
    public interface ISelectable
    {
        Sprite Icon { get; }

        Collider HitBox { get; }

        GameObject GameObject { get; }
    }
}