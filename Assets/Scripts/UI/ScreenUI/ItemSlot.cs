using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ItemSlot : MonoBehaviour, IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Image Image;
    public Sprite DefaultIcon;
    public StoredItem StoredItem;

    private bool _isDraged;

    public int? Id
    { 
        get { return _id; } 
        set 
        {
            if (_id == null)
                _id = value;
        } 
    }

    private int? _id = null;

    public void OnDrag(PointerEventData eventData) { }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_isDraged & ScreenUI.Instance.IsMouseEscapeUI())
            Events.OnItemDrop.Publish((int)_id);

        _isDraged = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _isDraged = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click");
    }

    public void SetDefaultItem()
    {
        Image.sprite = DefaultIcon;
        StoredItem = null;
    }

    public void SetItem(StoredItem _item)
    {
        Image.sprite = _item.Icon;
        StoredItem = _item;
    }
}