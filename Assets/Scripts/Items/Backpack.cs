using UnityEditor;
using UnityEngine;
using System.Collections.Generic;


public class Backpack : MonoBehaviour
{
    public int MaxSize;
    public Unit Owner;
    public Dictionary<int, ItemPrefab> Items = new Dictionary<int, ItemPrefab>();

    private void Start()
    {
        MaxSize = ItemsPool.Instance.DefaultBackpackSize;
    }

    public void AddToBackpack(ItemPrefab _item)
    {
        int? _emptySlot = TryGetEmptySlot();

        if (_emptySlot != null)
            Items[(int)_emptySlot] = _item;
    }

    public void DropItem(int _itemId)
    {
        if (Items.ContainsKey(_itemId)) 
        {
            ItemPrefab _item = Items[_itemId];
            ((StoredItem)_item.BaseItem).Drop(this, _item);
            Items.Remove(_itemId);
            ItemsPool.Instance.MoveToScene(_item);
        }
    }

    public Dictionary<int, ItemPrefab> GetItems()
    {
        return Items;
    }

    private int? TryGetEmptySlot()
    {
        for (int i = 0; i < MaxSize; i++)
            if (!Items.ContainsKey(i))
                return i;

        return null;
    }
}