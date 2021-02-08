using UnityEngine;
using System.Linq;


public abstract class Order 
{
    public virtual void PointSelected(Character _character, Vector3 _point) { }
    public virtual void ItemSelected(Character _character, ItemPrefab _item, out bool _isBreak) { _isBreak = false; }
    public virtual void CharacterSelected(Character _character, IDamageable _damageable) { }
}

public class DefaultOrder : Order
{
    public override void PointSelected(Character _character, Vector3 _point) 
    {
         _character.MoveOrder(_point);
    }

    public override void ItemSelected(Character _character, ItemPrefab _item, out bool _isBreak)
    {
        _isBreak = true;
        _item.Collect(_character);
    }

    public override void CharacterSelected(Character _character, IDamageable _damageable)
    {
        if (AllianceSystem.Instance.GetEnemyTags(GameManager.Instance.PlayersTag).Contains(_damageable.GameObject.tag))
            _character.Attack(_damageable);
        else
            _character.MoveToDamageable(_damageable);
    }
}

public class AttackOrder : Order
{
    public override void PointSelected(Character _character, Vector3 _point)
    {
        _character.AttackMove(_point);
    }

    public override void ItemSelected(Character _character, ItemPrefab _item, out bool _isBreak) 
    {
        _isBreak = false;
        _character.Attack(_item);
    }

    public override void CharacterSelected(Character _character, IDamageable _damageable)
    {
        _character.Attack(_damageable);
    }
}


