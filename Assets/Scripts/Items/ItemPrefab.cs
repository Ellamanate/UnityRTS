using UnityEngine;


public class ItemPrefab : GameEntity
{
    public BaseItem BaseItem;
    public Unit Owner { get => _owner; }
    public override Sprite Icon { get => BaseItem.Icon; }

    private Unit _owner;

    public void Collect(Unit _collectorner)
    {
        _owner = _collectorner;
        BaseItem.Collect(_collectorner, this);
    }

    public override void WhenStart()
    {
        CurrentHP = MaxHP;
        ItemsPool.Instance.AddItem(this);
    }
}