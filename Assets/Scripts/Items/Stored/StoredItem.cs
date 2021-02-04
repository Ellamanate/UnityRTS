public class StoredItem : BaseItem
{
    public override void Collect(Unit _owner, ItemPrefab _prefab) 
    {
        if(_owner.GetComponent<Backpack>() != null)
        {
            _owner.GetComponent<Backpack>().AddToBackpack(_prefab);
            ItemsPool.Instance.BackToPool(_prefab);
            base.Collect(_owner, _prefab);
        }
    }

    public virtual void Drop(Backpack _backpack, ItemPrefab _prefab) { }
}