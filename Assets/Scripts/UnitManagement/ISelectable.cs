using UnityEngine;


public interface ISelectable
{
    Sprite Icon { get; }

    Collider HitBox { get; }

    WorldUIContainer WorldUIContainer { get; }

    GameObject GameObject { get; }
}