using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ItemSlot : MonoBehaviour, IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Sprite CurrentIcon { get => _image.sprite; }
    public Sprite DefaultIcon { get => _defaultIcon; }
    public StoredItem StoredItem { get => _storedItem; }
    public int? Id { get => _id; set => OnSetId(value); }

    [SerializeField] private Sprite _defaultIcon;
    [SerializeField] private Image _image;
    private StoredItem _storedItem;
    private bool _isDraged;
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
        _image.sprite = _defaultIcon;
        _storedItem = null;
    }

    public void SetItem(StoredItem _item)
    {
        _image.sprite = _item.Icon;
        _storedItem = _item;
    }

    private void OnSetId(int? _newId)
    {
        if (_id == null)
            _id = _newId;
    }
}