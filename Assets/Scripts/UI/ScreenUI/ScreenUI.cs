using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScreenUI : Singleton<ScreenUI>
{
    public Canvas Canvas;
    public EventSystem m_EventSystem;
    public GraphicRaycaster m_Raycaster;
    public Image HealthBar;
    public Image ManaBar;
    public Image SelectedIcon;
    public Sprite DefaultIcon;
    public RectTransform ItemsPanel;
    public RectTransform SkillsPanel;
    public SkillUI SkillPrefab;
    public ItemSlot ItemSlotPrefab;
    public RectTransform Pool;

    private ISelectable _selectedUnit;
    private BackpackPainter _backpackPainter;
    private SkillPainter _skillPainter;

    public void OnSelected(IReadOnlyList<ISelectable> _selectedList)
    {
        if (_selectedList.Count > 0)
        {
            _selectedUnit = _selectedList[0];
            SelectedIcon.sprite = _selectedUnit.Icon;

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
                HealthBar.fillAmount = (float)_damageable.CurrentHP / (float)_damageable.MaxHP;
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
        PointerEventData m_PointerEventData = new PointerEventData(m_EventSystem) { position = Input.mousePosition };
        m_Raycaster.Raycast(m_PointerEventData, results);

        return results.Count == 0;
    }

    private void Awake()
    {
        SelectedIcon.sprite = DefaultIcon;
    }

    private void Start()
    {
        _backpackPainter = new BackpackPainter(ItemsPanel, ItemSlotPrefab);
        _skillPainter = new SkillPainter(SkillPrefab, Pool, SkillsPanel, ManaBar);
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
        SelectedIcon.sprite = DefaultIcon;
        _backpackPainter.HideBackpack();
        _skillPainter.DropTarget();
        ResetHealthBarToZero();
    }

    private void ResetHealthBarToZero()
    {
        HealthBar.fillAmount = 0;
    }
}