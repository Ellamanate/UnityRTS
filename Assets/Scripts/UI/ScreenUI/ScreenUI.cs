using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScreenUI : Singleton<ScreenUI>
{
    [SerializeField] private Canvas _сanvas;
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private GraphicRaycaster _raycaster;
    [SerializeField] private Image _healthBar;
    [SerializeField] private Image _manaBar;
    [SerializeField] private Image _selectedIcon;
    [SerializeField] private Sprite _defaultIcon;
    [SerializeField] private RectTransform _itemsPanel;
    [SerializeField] private RectTransform _skillsPanel;
    [SerializeField] private SkillUI _skillPrefab;
    [SerializeField] private ItemSlot _itemSlotPrefab;
    [SerializeField] private RectTransform _pool;
    private ISelectable _selectedUnit;
    private BackpackPainter _backpackPainter;
    private SkillPainter _skillPainter;

    public void OnSelected(IReadOnlyList<ISelectable> _selectedList)
    {
        if (_selectedList.Count > 0)
        {
            _selectedUnit = _selectedList[0];
            _selectedIcon.sprite = _selectedUnit.Icon;

            _skillPainter.PaintSkills(_selectedList[0] as Unit);
            _backpackPainter.PaintItems(_selectedList[0] as Unit);

            UpdateHealthBar(_selectedUnit as IDamageable);
        }
        else { DropTarget(); }
    }

    public void UpdateHealthBar(IDamageable _damageable)
    {
        if (_selectedUnit != null & _damageable != null)
            if (_damageable.GameObject == _selectedUnit.GameObject)
                _healthBar.fillAmount = (float)_damageable.CurrentHP / (float)_damageable.MaxHP;
    }

    public void UpdateManaBar(SkillCaster _caster)
    {
        if (_selectedUnit != null & _caster != null)
            if (_caster == _selectedUnit.GameObject.GetComponent<SkillCaster>())
                _skillPainter.UpdateManaBar(_caster);
    }

    public void OnSkillActivate(SkillManager _skillManager)
    {
        if (_skillManager != null & _selectedUnit != null)
            if (_skillManager.Caster == _selectedUnit.GameObject.GetComponent<SkillCaster>())
                _skillPainter.UpdateSkillsCoolDown(_skillManager);
    }

    public bool IsMouseEscapeUI()
    {
        List<RaycastResult> results = new List<RaycastResult>();
        PointerEventData m_PointerEventData = new PointerEventData(_eventSystem) { position = Input.mousePosition };
        _raycaster.Raycast(m_PointerEventData, results);

        return results.Count == 0;
    }

    private void Awake()
    {
        _selectedIcon.sprite = _defaultIcon;
    }

    private void Start()
    {
        _backpackPainter = new BackpackPainter(_itemsPanel, _itemSlotPrefab);
        _skillPainter = new SkillPainter(_skillPrefab, _pool, _skillsPanel, _manaBar);
    }

    private void OnEnable()
    {
        Events.OnSelectedChange.Subscribe(OnSelected);
        Events.OnSkillActivate.Subscribe(OnSkillActivate);
        Events.OnDamageableHPChange.Subscribe(UpdateHealthBar);
        Events.OnSkillCasterMPChange.Subscribe(UpdateManaBar);
        Events.OnUnitDestroy.Subscribe(OnUnitDestroy);
        Events.OnItemCollect.Subscribe(OnItemCollect);
        Events.OnItemDrop.Subscribe(OnItemDrop);
    }

    private void OnDisable()
    {
        Events.OnSelectedChange.UnSubscribe(OnSelected);
        Events.OnSkillActivate.UnSubscribe(OnSkillActivate);
        Events.OnDamageableHPChange.UnSubscribe(UpdateHealthBar);
        Events.OnSkillCasterMPChange.UnSubscribe(UpdateManaBar);
        Events.OnUnitDestroy.UnSubscribe(OnUnitDestroy);
        Events.OnItemCollect.UnSubscribe(OnItemCollect);
        Events.OnItemDrop.UnSubscribe(OnItemDrop);
    }

    private void OnItemCollect(BaseItem _item)
    {
        _backpackPainter.PaintItems(_selectedUnit as Unit);
    }

    private void OnItemDrop(int _id)
    {
        _backpackPainter.RemoveItemById(_id);
    }

    private void OnUnitDestroy(IDamageable _destroyed)
    {
        if (_destroyed == _selectedUnit as IDamageable)
            DropTarget();
    }

    private void DropTarget()
    {
        _selectedUnit = null;
        _selectedIcon.sprite = _defaultIcon;
        _backpackPainter.HideBackpack();
        _skillPainter.DropTarget();
        ResetHealthBarToZero();
    }

    private void ResetHealthBarToZero()
    {
        _healthBar.fillAmount = 0;
    }
}