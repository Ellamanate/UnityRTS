public class ActiveItem : BaseItem
{
    public override void Collect(Unit _owner, ItemPrefab _prefab) 
    {
        base.Collect(_owner, _prefab);
        ItemsPool.Instance.DestroyItem(_prefab); 
    }
}