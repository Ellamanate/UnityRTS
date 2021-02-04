using System.Collections;
using UnityEngine;


[RequireComponent(typeof(UnitSelector))]
public class UnitManager : Singleton<UnitManager>
{
    [SerializeField] private string[] _skillHotKeys;
    private SkillManager _currentSkillManager;
    private AttackOrder _attackOrder;
    private DefaultOrder _defaultOrder;
    private UnitOrdering _unitOrdering;
    private bool _selectingSkillTarget = false;
    private bool _selectingTargetToAttack = false;

    public void SkillTargetSelecting(SkillManager _skillManager)
    {
        _selectingSkillTarget = true;
        _currentSkillManager = _skillManager;
    }

    private void Awake()
    {
        _attackOrder = new AttackOrder();
        _defaultOrder = new DefaultOrder();
        _unitOrdering = new UnitOrdering(_skillHotKeys);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LeftClick();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            RightClick();
        }
        else if (UnitSelector.Instance.CanControlSelected)
        {
            KeyDown();
        }
    }

    private void LeftClick()
    {
        if (_selectingSkillTarget && _currentSkillManager != null)
        {
            _selectingSkillTarget = false;
            _currentSkillManager.SelectTarget();
        }
        else if (_selectingTargetToAttack && UnitSelector.Instance.CanControlSelected)
        {
            _selectingTargetToAttack = false;
            _unitOrdering.GiveOrder(UnitSelector.Instance.SelectedUnits, _attackOrder);
        }
        else
        {
            UnitSelector.Instance.Select();
        }
    }

    private void RightClick()
    {
        if (DropState(ref _selectingSkillTarget) || DropState(ref _selectingTargetToAttack))
        {
            return;
        }
        else if (UnitSelector.Instance.CanControlSelected)
        {
            _unitOrdering.GiveOrder(UnitSelector.Instance.SelectedUnits, _defaultOrder);
        }
    }

    private void KeyDown()
    {
        if (!_selectingSkillTarget)
        {
            _unitOrdering.IsPressSkillHotkey(UnitSelector.Instance.SelectedUnits);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            _selectingTargetToAttack = true;
        }
    }

    private bool DropState(ref bool _state)
    {
        if (_state)
        {
            _state = false;
            return true;
        }

        return false;
    }
}