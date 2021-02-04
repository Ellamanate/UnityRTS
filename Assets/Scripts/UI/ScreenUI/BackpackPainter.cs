using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class BackpackPainter
{
    private RectTransform _itemsPanel;
    private Dictionary<int, ItemSlot> _slots = new Dictionary<int, ItemSlot>();
    private Backpack _currentBackpack;

    public BackpackPainter(RectTransform _panel, ItemSlot _slotPrefab)
    {
        _itemsPanel = _panel;

        for (int i = 0; i < ItemsPool.Instance.DefaultBackpackSize; i++)
        {
            ItemSlot _newSlot = GameObject.Instantiate(_slotPrefab, _itemsPanel);
            _newSlot.Id = i;
            _slots[i] = _newSlot;
        }
    }

    public void PaintItems(Unit _selectedUnit)
    {
        if (_selectedUnit != null)
        {
            if (!_itemsPanel.gameObject.activeSelf)
                _itemsPanel.gameObject.SetActive(true);

            if (_selectedUnit.GetComponent<Backpack>() != null)
            {
                _currentBackpack = _selectedUnit.GetComponent<Backpack>();
                Dictionary<int, ItemPrefab> _items = _currentBackpack.GetItems();

                for (int i = 0; i < _slots.Count; i++)
                {
                    if (_items.ContainsKey(i))
                        _slots[i].SetItem((StoredItem)_items[i].BaseItem);
                    else
                        _slots[i].SetDefaultItem();
                }
            }
            else ClearBackpack();
        }
        else _itemsPanel.gameObject.SetActive(false);
    }

    public void RemoveItemById(int _id)
    {
        _slots[_id].SetDefaultItem();
        _currentBackpack.DropItem(_id);
    }

    public void ClearBackpack()
    {
        for (int i = 0; i < _slots.Count; i++)
            _slots[i].SetDefaultItem();
    }

    public void HideBackpack()
    {
        _itemsPanel.gameObject.SetActive(false);
    }
}