using UnityEngine;
using System.Collections.Generic;


public class ItemsPool : Singleton<ItemsPool>
{
    [HideInInspector] public List<ItemPrefab> Items = new List<ItemPrefab>();
    public Transform PoolTransform;
    public int DefaultBackpackSize;

    public void AddItem(ItemPrefab _item)
    {
        Items.Add(_item);
    }

    public void RemoveItem(ItemPrefab _item)
    {
        Items.Remove(_item);
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