using UnityEngine;
using System.Collections.Generic;


public class ItemsPool : Singleton<ItemsPool>
{
    public int DefaultBackpackSize { get => _defaultBackpackSize; }
    public Transform PoolTransform { get => _poolTransform; }
    public IReadOnlyCollection<ItemPrefab> Items { get => _items.AsReadOnly(); }

    [SerializeField] private int _defaultBackpackSize;
    [SerializeField] private Transform _poolTransform;
    private List<ItemPrefab> _items = new List<ItemPrefab>();

    public void AddItem(ItemPrefab _item)
    {
        _items.Add(_item);
    }

    public void RemoveItem(ItemPrefab _item)
    {
        _items.Remove(_item);
    }

    public void BackToPool(ItemPrefab _item)
    {
        _item.GetComponent<Transform>().SetParent(PoolTransform);
    }

    public void MoveToScene(ItemPrefab _item)
    {
        Transform _itemTransform = _item.GetComponent<Transform>();
        _itemTransform.SetParent(null);

        if (Raycaster.Instance.DefaultMouseRaycast(out RaycastHit _hitInfo))
            _itemTransform.position = _hitInfo.point;
    }

    public void DestroyItem(ItemPrefab _item)
    {
        _item.Destroy();
    }
}