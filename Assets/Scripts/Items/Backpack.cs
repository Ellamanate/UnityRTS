using UnityEditor;
using UnityEngine;
using System.Collections.Generic;


public class Backpack : MonoBehaviour
{
    public int MaxSize { get => _maxSize; }
    public Unit Owner { get => _owner; }
    public IReadOnlyDictionary<int, ItemPrefab> Items { get => _items; }

    [SerializeField] private int _maxSize;
    private Unit _owner;
    private Dictionary<int, ItemPrefab> _items = new Dictionary<int, ItemPrefab>();

    private void Start()
    {
        _maxSize = ItemsPool.Instance.DefaultBackpackSize;
    }

    public void AddToBackpack(ItemPrefab _item)
    {
        int? _emptySlot = TryGetEmptySlot();

        if (_emptySlot != null)
            _items[(int)_emptySlot] = _item;
    }

    public void DropItem(int _itemId)
    {
        if (_items.ContainsKey(_itemId)) 
        {
            ItemPrefab _item = _items[_itemId];
            ((StoredItem)_item.BaseItem).Drop(this, _item);
            _items.Remove(_itemId);
            ItemsPool.Instance.MoveToScene(_item);
        }
    }

    public IReadOnlyDictionary<int, ItemPrefab> GetItems()
    {
        return Items;
    }

    private int? TryGetEmptySlot()
    {
        for (int i = 0; i < MaxSize; i++)
            if (!_items.ContainsKey(i))
                return i;

        return null;
    }
}